import os
import sys
import requests

path = sys.argv[1]

for filename in os.listdir(path):
    if filename.endswith('.wav'):
        url = 'http://localhost:5000/model/predict?start_time=0'
        files = {'audio': (filename, open(os.path.join(path, filename), 'rb'), 'audio/wav')}
        headers = {'accept': 'application/json'}
        response = requests.post(url, headers=headers, files=files)
        response_txt = response.json()["predictions"]
        if response_txt[1]["label"] == "Car" or response_txt[1]["label"] == "Engine":
            print(f'{response_txt[0]["label"]:<10} {response_txt[0]["probability"]:.2f} | {response_txt[1]["label"]:<10} {response_txt[1]["probability"]:.2f} | {response_txt[2]["probability"]:.2f} {response_txt[2]["label"]:<30} {filename}')