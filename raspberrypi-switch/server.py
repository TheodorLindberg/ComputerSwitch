import time
from flask import Flask
from flask import Response

import RPi.GPIO as GPIO
GPIO.cleanup()
GPIO.setmode(GPIO.BCM)
switchPin = 3
GPIO.setup(switchPin, GPIO.OUT)
GPIO.output(switchPin, GPIO.LOW)

from threading import Thread

app = Flask(__name__)


switching = False


def high():
    global switching
    if switching: return
    switching = True
    print("HIGH")
    GPIO.output(switchPin, GPIO.HIGH)
    time.sleep(1)
    GPIO.output(switchPin, GPIO.LOW)
    switching = False

def low():
    global switching
    if switching: return
    switching = True
    print("LOW")
    GPIO.output(switchPin, GPIO.HIGH)
    time.sleep(4)
    GPIO.output(switchPin, GPIO.LOW)
    switching = False

@app.route("/")
def index():
    return '<body><form action="/on"><button type="submit">On</button></form><form action="/off"><button type="submit">Off</button></form></body>'

@app.route("/on")
def on():
    resp = Response("Turning on<script>window.location='/'</script>")

    thread = Thread(target = high)
    thread.start()
    return resp

@app.route("/off")
def off():
    resp = Response("Turning Off<script>window.location='/'</script>")
    thread = Thread(target = low)
    thread.start()
    return resp

if __name__ == '__main__':
    app.debug = True
    app.run(host="192.168.10.193", port=5000) #go to http://localhost:5000/ to view the page.
