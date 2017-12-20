from flask import Flask
from flask_pymongo import PyMongo
from flask import Response
import random
import requests
from flask import request
import json
from itsdangerous import (TimedJSONWebSignatureSerializer
                          as Serializer, BadSignature, SignatureExpired)
from flask import jsonify
from bson.objectid import ObjectId
from functools import wraps
import uuid
from bson import json_util


app = Flask(__name__)
app.config['SECRET_KEY'] = "secret"
app.config['MONGO_DBNAME'] = "tasker_db"
app.config['MONGO_URI'] = "mongodb://localhost:27017"
mongo = PyMongo(app)


def to_json(data):
    """Convert Mongo object(s) to JSON"""
    return json.dumps(data, default=json_util.default)

#region Security

def generate_auth_token(id, expiration=600):
    ss = str(id)
    s = Serializer(app.config['SECRET_KEY'], expires_in=expiration)

    return s.dumps({'id': ss})


def authorize(f):
    @wraps(f)
    def wrapper(*args, **kwargs):
        if not 'Authorization' in request.headers:
            return Response(status="401")

        data = request.headers['Authorization']
        user = verify_auth_token(data)

        if not user:
            return Response(status="404")

        return f(user)

    return wrapper


def verify_auth_token(token):
    s = Serializer(app.config['SECRET_KEY'])
    t = token.replace('\'', '')[1:]
    try:
        data = s.loads(t)
    except SignatureExpired:
        return None  # valid token, but expired
    except BadSignature:
        return None  # invalid token

    dd = data['id']
    user = mongo.db.users.find_one({'_id': ObjectId(dd)})
    return user

#endregion


@app.route('/')
def home_page():
    user = mongo.db.users
    return "Success!"


# region Registration

@app.route('/register', methods=['POST'])
def register():
    req = request.get_json(silent=True)
    rnd = random.randrange(1000, 9999)

    if mongo.db.users.find_one({'phone': req['phone']}):
        return Response(
            "Указанный номер зарегистрирован",
            status_code=409,
            content_type="utf-8")

    tmp_users = mongo.db.template_users
    user = tmp_users.find_one({'phone': req['phone']})
    if not tmp_users.find_one_and_update({'phone': req['phone']}, {'$set': {'code': rnd}}, upsert=True):
        tmp_users.insert({'phone': req['phone'], 'code': rnd})


    #remove this
    return Response(status="200")

    r = requests.post('https://sms.ru/sms/send?api_id=840B3593-66E9-5AB4-4965-0B9589019F3A&to=' + str(
        req['phone']) + '&msg=Код%20для%20регистрации:%20' + str(rnd) + '&json=1')

    return Response(
        r.text,
        status=r.status_code,
        content_type=r.headers['content-type'])


@app.route('/register/confirm', methods=['POST'])
def finish_registration():
    req = request.get_json(silent=True)

    if mongo.db.users.find_one({'phone': req['phone']}):
        return Response("Указанный номер уже зарегистирован", status="409", content_type="utf-8")

    tmp_users = mongo.db.template_users
    if tmp_users.find_one({'phone': req['phone'], 'code': int(req['code'])}):
        to_insert = {'phone': req['phone'], 'profile': {'first_name': '', 'last_name': '', 'birth_date': None}}
        mongo.db.users.insert_one(
            {'phone': req['phone'], 'profile': {'first_name': '', 'last_name': '', 'birth_date': None}})
        return Response(json.dumps(to_insert), status="200", content_type='application/json')
    else:
        return Response(status="404")


@app.route('/login', methods=['POST'])
def tasks():
    req = request.get_json(silent=True)

    user = mongo.db.users.find_one({'phone': req['phone']})
    if user:
        token = generate_auth_token(user['_id'])
        return Response(json.dumps({'token': str(token)}), status="200", content_type='application/json')
    else:
        return Response(status="404")
    
    


# endregion

#region Task

@app.route('/tasks', methods=['GET'])
@authorize
def get_all_task(user):
    tasks = mongo.db.tasks.find({'user_id': str(user['_id'])})
    json_result = []
    for task in tasks:
      json_result.append(task)

    result = to_json(json_result)

    return Response(result, status="200", content_type='application/json')


@app.route('/task', methods=['POST'])
@authorize
def add_task(user):
    req = request.get_json(silent=True)

    mongo.db.tasks.insert({'data': req['data'],
                           'date': req['date'],
                           'guid': req['guid'],
                           'user_id':  str(user['_id'])})

    return Response(status="200")


@app.route('/task', methods=['PUT'])
@authorize
def update_task(user):
    req = request.get_json(silent=True)

    mongo.db.tasks.update({'guid': req['guid']},
                          {"$set" : {'data': req['data'],
                                     'date': req['date']}})

    return Response(status="200")

@app.route('/task', methods=['GET'])
@authorize
def get_task(user):
    req = request.get_json(silent=True)
    id = request.args.get('id')

    task = mongo.db.tasks.find_one({'guid' : id})

    return Response(to_json(task), status="200", content_type='application/json')


@app.route('/task', methods=['DELETE'])
@authorize
def delete_task(user):
    req = request.get_json(silent=True)
    mongo.db.tasks.delete_one({'guid' : req['guid']})

    return Response(status="200")

#endregion


if __name__ == "__main__":
    app.run()
