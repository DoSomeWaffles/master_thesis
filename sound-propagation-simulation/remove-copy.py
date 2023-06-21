# delete every files that has copy.wav in its name in every subfolders in the given folder

import os
import argparse

parser = argparse.ArgumentParser(description='Delete copy.wav')
parser.add_argument('--folder', type=str, default='dataset_real_data\\v2-wav', help='folder name')
args = parser.parse_args()

# get every files in the given folder
files = []
for root, dirs, fs in os.walk(args.folder):
    for f in fs:
        files.append(os.path.join(root, f))

# delete every copy.wav files
count = 0
for f in files:
    if 'copy.wav' in f:
        print('removing: ', f)
        os.remove(f)
        count += 1
print('removed: ', count, ' files')
