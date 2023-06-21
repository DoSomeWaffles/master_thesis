# take folder as input and take all inputs in test and train set and shuffle them into test and train with the given ratio

import os
import random
import shutil
import argparse

parser = argparse.ArgumentParser(description='Shuffle dataset')
parser.add_argument('--folder', type=str, default='dataset_real_data\\v2-wav', help='folder name')
parser.add_argument('--ratio', type=float, default=0.2, help='ratio of test set')
args = parser.parse_args()

folder = args.folder
ratio = args.ratio

# list classes in folder test and train
classes = os.listdir(folder + '\\train')

print('classes: ', classes)

# create folder for each class in folder test 
for c in classes:
    if not os.path.exists(folder + '\\test\\' + c):
        os.makedirs(folder + '\\test\\' + c)

# for each class in folder test and train, shuffle files and move them to test and train
counter = 0
files_train = []
files_test = []
for c in classes:
    files_train = os.listdir(folder + '\\train\\' + c)
    # append files in test to files in train
    files_test = os.listdir(folder + '\\test\\' + c)

    random.shuffle(files_train)
    random.shuffle(files_test)

    # don't move files that end with stereo.wav
    files_train = [f for f in files_train if not f.endswith('stereo.wav')]
    files_test = [f for f in files_test if not f.endswith('stereo.wav')]

    # move files from train to test
    for i in range(int(len(files_train) * ratio)):
        shutil.move(folder + '\\train\\' + c + '\\' + files_train[i], folder + '\\test\\' + c + '\\')
        counter += 1
    # move files from test to train
    for i in range(int(len(files_test) * (1 - ratio))):
        shutil.move(folder + '\\test\\' + c + '\\' + files_test[i], folder + '\\train\\' + c + '\\')
        counter += 1
        
# count files in test and train
files_train = []
files_test = []
for c in classes:
    files_train += os.listdir(folder + '\\train\\' + c)
    files_test += os.listdir(folder + '\\test\\' + c)

print('files in train: ', len(files_train))
print('files in test: ', len(files_test))
print('total files: ', len(files_train) + len(files_test))
print('ratio: ', len(files_test) / (len(files_train) + len(files_test)))

# get the ration between classes
for c in classes:
    files_train = os.listdir(folder + '\\train\\' + c)
    files_test = os.listdir(folder + '\\test\\' + c)
    print('total files for class ' + c + ': ', len(files_train) + len(files_test))


