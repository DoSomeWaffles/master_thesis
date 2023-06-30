import os
import argparse
from moviepy.editor import *

parser = argparse.ArgumentParser(description='Extract audio from all videos in a folder and save them in another folder.')
parser.add_argument('input_folder', type=str, help='path to input folder')
parser.add_argument('output_folder', type=str, help='path to output folder')
args = parser.parse_args()



if not os.path.exists(args.output_folder):
    os.makedirs(args.output_folder)

for filename in os.listdir(args.input_folder):
    if filename.endswith('.mp4'):
        video_path = os.path.join(args.input_folder, filename)
        audio_path = os.path.join(args.output_folder, filename[:-4] + '.wav')
        
        videoclip = VideoFileClip(video_path)
        audioclip = videoclip.audio
        
        audioclip.write_audiofile(audio_path)
        
        audioclip.close()
        videoclip.close()
