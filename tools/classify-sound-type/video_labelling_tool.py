import os
import cv2
import argparse
import shutil

# Define the argument parser
parser = argparse.ArgumentParser(description='Video Labeling Tool')

# Add the folder path argument
parser.add_argument('folder_path', type=str,
                    help='Path to the folder containing videos')

# Parse the arguments
args = parser.parse_args()

# Define the folder path where videos are stored
folder_path = args.folder_path

# Define the label folders
label_folders = {
    '1': folder_path[:-1]+'left_to_right',
    '2': folder_path[:-1]+'right_to_left',
    '3': folder_path[:-1]+'multiple_cars',
    '4': folder_path[:-1]+'no_car',
}


# Create the label folders if they don't exist
for label_folder in label_folders.values():
    os.makedirs(label_folder, exist_ok=True)

# Get the list of all video files in the folder
video_files = [f for f in os.listdir(folder_path) if f.endswith('.mp4')]

# Loop through each video file and play it
last_video_saved_filename = ""
counter = 0
for video_file in video_files:
    # Open the video file
    cap = cv2.VideoCapture(os.path.join(folder_path, video_file))

    # Get the total number of frames in the video file
    total_frames = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))

    # Loop through each frame of the video file and display it
    while True:
        ret, frame = cap.read()
        if not ret:
            break

        cv2.imshow("Video", frame)

        if cv2.waitKey(1) == ord('q'):
            break

    key = cv2.waitKey(0)
    # Check which key was pressed by the user and move the video file to the corresponding label folder

    if key == ord('a'):
        label_folder = label_folders['1']
    elif key == ord('l'):
        label_folder = label_folders['2']
    elif key == ord('m'):
        label_folder = label_folders['3']
    elif key == ord('n'):
        label_folder = label_folders['4']
    elif key == ord('s'):
        # delete the video file from last video saved folder
        if last_video_saved_filename != "":
            os.remove(os.path.join(folder_path, last_video_saved_filename))
            print("Deleted video file: ", last_video_saved_filename,
                  " from folder: ", folder_path)
            continue
    else:
        counter -= 1
    shutil.copy(os.path.join(folder_path, video_file),
                os.path.join(label_folder, video_file))
    print("Copied video file: ", video_file, " to folder: ", label_folder)
    print("Number of video saved:", counter)
    counter += 1
    last_video_saved_filename = video_file
    # Release the capture object and close all windows

cap.release()
cv2.destroyAllWindows()
