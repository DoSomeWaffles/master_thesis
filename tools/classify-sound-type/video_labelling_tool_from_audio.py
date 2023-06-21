import os
import cv2
import argparse
import shutil
import random
from moviepy.editor import VideoFileClip
import keyboard
# Define the argument parser
parser = argparse.ArgumentParser(description='Video Labeling Tool')

# Add the folder path argument
parser.add_argument('folder_path', type=str,
                    help='Path to the folder containing videos')
# Add the number of files to skip argument
parser.add_argument('--skip', type=int, default=0,
                    help='Number of files to skip')

# Parse the arguments
args = parser.parse_args()
# Define the folder path where videos are stored

folder_path = args.folder_path
num_files_to_skip = args.skip
# Define the label folders
label_folders = {
    '1': "dataset/labeled-from-audio/"+folder_path[:-1]+'-left_to_right-from-sound',
    '2': "dataset/labeled-from-audio/"+folder_path[:-1]+'-right_to_left-from-sound',
    '3': "dataset/labeled-from-audio/"+folder_path[:-1]+'-multiple_cars-from-sound',
    '4': "dataset/labeled-from-audio/"+folder_path[:-1]+'-no_car-from-sound',
}

# Create the label folders if they don't exist
for label_folder in label_folders.values():
    os.makedirs(label_folder, exist_ok=True)

# Get the list of all video files in the folder
video_files = [f for f in os.listdir(folder_path) if f.endswith('.mp4')]

already_labeled_folders = [f for f in os.listdir("dataset/labeled-from-video")]
already_labeled_files = []
for label_folder in already_labeled_folders:
    files=[f for f in os.listdir("dataset/labeled-from-video/"+label_folder) if f.endswith('.mp4')]
    for file in files:
        already_labeled_files.append(file)
# keep only the video files that are already labeled
print(video_files[0])
print(already_labeled_files[1])
c=0
r=0
for video in video_files:
    if video in already_labeled_files:
        c+=1
    else:
        r+=1
print(c,r)
video_files = [f for f in video_files if f in already_labeled_files]
print(len(video_files))
# Loop through each video file and play it
last_video_saved_filename = ""
counter = 0
# shuffle the video files
random.shuffle(video_files)
for video_file in video_files:
    counter += 1
    if counter <= num_files_to_skip:
        continue
    clip = VideoFileClip(os.path.join(folder_path, video_file))
    clip.preview()
    print("Playing audio of video file: ", video_file)
    cv2.destroyAllWindows()
    key = ""
    while 1:
        key = keyboard.read_key()
        # check if a key is pressed
        if key != "":
            break

    # Check which key was pressed by the user and move the video file to the corresponding label folder
    print(key)
    if key == ('d'):
        label_folder = label_folders['1']
    elif key == ('a'):
        label_folder = label_folders['2']
    elif key == ('w'):
        label_folder = label_folders['3']
    elif key == ('s'):
        label_folder = label_folders['4']
    elif key == ('r'):
        # delete the video file from last video saved folder
        if last_video_saved_filename != "":
            os.remove(os.path.join(folder_path, last_video_saved_filename))
            print("Deleted video file: ", last_video_saved_filename,
                  " from folder: ", folder_path)
            continue
    else:
        continue
    shutil.copy(os.path.join(folder_path, video_file),
                os.path.join(label_folder, video_file))
    print("Copied video file: ", video_file, " to folder: ", label_folder)
    print("Number of video saved:", counter)
    last_video_saved_filename = video_file

# Release the capture object and close all windows
cap.release()
cv2.destroyAllWindows()
