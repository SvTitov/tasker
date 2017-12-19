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


app = Flask(__name__)
app.config['SECRET_KEY'] = "super secret word"
app.config['MONGO_DBNAME'] = "tasker_db"
app.config['MONGO_URI'] = "mongodb://localhost:27017"
mongo = PyMongo(app)


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

        data = request.headers['Authorization'].encode('utf-8', 'ignore')
        user = verify_auth_token(data)
        return f(user, args, kwargs)

    return wrapper


def verify_auth_token(token):
    s = Serializer(app.config['SECRET_KEY'])
    try:
        data = s.loads(token)
    except SignatureExpired:
        return None  # valid token, but expired
    except BadSignature:
        return None  # invalid token

    user = mongo.db.find_one({'_id': ObjectId(data)})
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
    if tmp_users.find_one({'phone': req['phone'], 'code': req['code']}):
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


# endregion

#region Task

@app.route('/tasks', methods=['GET'])
@authorize
def get_all_task(user):
    return user.tasks


@app.route('/task', methods=['POST'])
@authorize
def add_task(user):
    req = request.get_json(silent=True)

    mongo.db.tasks.insert({'data': req['data'],
                           'date': req['date'],
                           'guid': req['guid'],
                           'user_id': user._id})

    return Response(status="200")

#endregion


if __name__ == "__main__":
    app.run()
