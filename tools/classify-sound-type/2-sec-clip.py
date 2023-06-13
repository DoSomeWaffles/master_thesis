# execute this command for each file in the directory and add the filename to the output file :ffmpeg -i output00000.mp4 -c:v libx264 -crf 22 -map 0 -segment_time 2 -reset_timestamps 1 -g 30 -sc_threshold 0 -force_key_frames "expr:gte(t,n_forced*2)" -f segment new/output%03d.mp4

import os
import sys
import subprocess

# get the directory name from the command line and the time in seconds
if len(sys.argv) != 3:
    print('Usage: python 2-sec-clip.py directory time')
    sys.exit(1)
directory = sys.argv[1]
time = sys.argv[2]

# get the list of files in the directory
files = os.listdir(directory)

# loop through each file in the directory
for file in files:
    subprocess.call(['ffmpeg', '-i', directory + '/' + file, '-c:v', 'libx264', '-crf', '22', '-map', '0', '-segment_time', time, '-reset_timestamps', '1', '-g', '30', '-sc_threshold', '0', '-force_key_frames', 'expr:gte(t,n_forced*'+str(time)+')', '-f', 'segment', directory[:-1]+'-2sec/' + file + '%05d.mp4'])

