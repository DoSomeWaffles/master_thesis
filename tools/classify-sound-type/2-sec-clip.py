import os
import sys
from moviepy.video.io.VideoFileClip import VideoFileClip

def split_videos_in_folder(folder_path):
    output_folder = os.path.join(folder_path, '2_sec_clips')
    os.makedirs(output_folder, exist_ok=True)
    for filename in os.listdir(folder_path):
        if filename.endswith('.mp4'):
            video_path = os.path.join(folder_path, filename)
            clips = split_video(video_path)
            for i, clip in enumerate(clips):
                clip.write_videofile(os.path.join(output_folder, f'{filename}_clip_{i}.mp4'))

def split_video(video_path):
    clip = VideoFileClip(video_path)
    duration = clip.duration
    clips = []
    for i in range(0, int(duration), 2):
        start_time = i
        end_time = min(i + 2, duration)
        clips.append(clip.subclip(start_time, end_time))
    return clips

if __name__ == '__main__':
    folder_path = sys.argv[1]
    split_videos_in_folder(folder_path)
