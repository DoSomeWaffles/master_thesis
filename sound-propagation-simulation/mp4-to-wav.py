# for each files in every folders in the given folder, convert them to wav

import os
import argparse
import subprocess

parser = argparse.ArgumentParser(description='Convert mp4 to wav')
parser.add_argument('--folder', type=str, default='dataset_real_data\\v2', help='folder name')
args = parser.parse_args()

# get every files in the given folder
files = []
for root, dirs, fs in os.walk(args.folder):
    for f in fs:
        files.append(os.path.join(root, f))

# convert every mp4 files to wav
for f in files:
    if f.endswith('.mp4'):
        print('converting: ', f)
        subprocess.call(['ffmpeg', '-i', f, f[:-3] + 'wav'])
        print('converted: ', f)

# remove every mp4 files
for f in files:
    if f.endswith('.mp4'):
        print('removing: ', f)
        os.remove(f)
        print('removed: ', f)
