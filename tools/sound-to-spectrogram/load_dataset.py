import os
import glob
import torchaudio
import librosa
import matplotlib.pyplot as plt
def plot_spectrogram(specgram, title=None, ylabel="freq_bin"):
    fig, axs = plt.subplots(1, 1)
    axs.set_title(title or "Spectrogram (db)")
    axs.set_ylabel(ylabel)
    axs.set_xlabel("frame")
    im = axs.imshow(librosa.power_to_db(specgram), origin="lower", aspect="auto")
    fig.colorbar(im, ax=axs)
    plt.show(block=False)

def count_label(dataset):
    label_count = {}
    for label in dataset['labels']:
        if label in label_count:
            label_count[label] += 1
        else:
            label_count[label] = 1
    return label_count

file_limit_train = 10000
file_limit_test = 10000

def load_dataset(path, is_test=False, simulated=False):
    # Create empty lists to store waveforms, sample rates and labels
    dataset = {'waveforms': [], 'sample_rates': [], 'labels': []}
    print("number of label:",len(os.listdir(path)))
    # Loop over the sub-folders in the root folder
    for folder in os.listdir(path):
        # Get the label from the folder name
        label = folder
        counter = 0
        # Loop over the audio files in each sub-folder
        for filename in glob.glob(os.path.join(path, folder, '*.wav')):
            if is_test and not is_test and filename.endswith("stereo.wav"):
                continue
            if simulated and not filename.endswith("stereo.wav"):
                continue
            counter += 1
            try:
                # Load an audio file as a tensor and its sample rate
                waveform, sample_rate = torchaudio.load(filename)
                if waveform.shape[1] < 86400:
                    continue
                if waveform.shape[1] > 86400:
                    waveform = waveform[:, :86400]
                # Append them to the lists
                dataset['waveforms'].append(waveform)
                dataset['sample_rates'].append(sample_rate)
                dataset['labels'].append(label)
                # Stop if we have enough audio files
                if counter >= file_limit_train and not is_test:
                    break
                if is_test and counter >= file_limit_test:
                    break
            except Exception as e:
                pass
    return dataset

import torch.nn as nn
from torchsummary import summary

import torch
import torch.nn as nn

class CNN(nn.Module):
    def __init__(self):
        super(CNN, self).__init__()
        self.conv1 = nn.Sequential(
            nn.Conv2d(
                in_channels=2,
                out_channels=8,
                kernel_size=5,
                stride=1,
                padding=2,
            ),
            nn.ReLU(),
            nn.MaxPool2d(kernel_size=5),
        )
        self.conv2 = nn.Sequential(
            nn.Conv2d(8, 16, 5, 1, 2),
            nn.ReLU(),
            nn.MaxPool2d(kernel_size=3),
        )
        self.conv3 = nn.Sequential(
            nn.Conv2d(16, 32, 5, 1, 2),
            nn.ReLU(),
            nn.MaxPool2d(kernel_size=2),
        )
        self.out = nn.Linear(1024, 4)
        # dropout layer (p=0.5)
        self.dropout = nn.Dropout(p=0.5)

    def forward(self, x):
        x = self.conv1(x)
        x = self.conv2(x)
        x = self.conv3(x)
        x = x.view(x.size(0), -1)
        output = self.out(x)
        return output, x    # return x for visualization