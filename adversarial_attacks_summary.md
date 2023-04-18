# Attacks on Audio Deep Learning Models

Audio deep learning models are widely used for various tasks such as speech recognition, speaker identification, emotion detection, and music generation. However, these models are also vulnerable to adversarial attacks, which are malicious inputs that are designed to fool or mislead the models. Adversarial attacks can have serious consequences for the security and reliability of audio applications, especially in safety-critical or sensitive domains.

In this report, we survey some of the recent research on adversarial attacks on audio deep learning models. We categorize the attacks based on their goals, methods, and scenarios. We also discuss some of the challenges and possible countermeasures for defending against these attacks.

## Goals of Adversarial Attacks

Adversarial attacks can have different goals depending on the attacker's motivation and objective. Some common goals are:

- **Targeted misclassification**: The attacker aims to make the model output a specific incorrect label or result for a given input. For example, an attacker may want to make a speech recognition model transcribe a benign audio as a malicious command.
- **Untargeted misclassification**: The attacker aims to make the model output any incorrect label or result for a given input. For example, an attacker may want to make a speaker identification model fail to recognize the true speaker of an audio.
- **Source/target confusion**: The attacker aims to make the model confuse the source or target of a given input. For example, an attacker may want to make a speaker verification model accept an impostor's voice as the genuine one, or vice versa.
- **Impersonation**: The attacker aims to make the model output a specific label or result that matches the attacker's identity or preference. For example, an attacker may want to make a voice conversion model generate an audio that sounds like the attacker's voice.
- **Degradation**: The attacker aims to degrade the performance or quality of the model for a given input. For example, an attacker may want to make a music generation model produce noisy or unpleasant sounds.

## Methods of Adversarial Attacks

Adversarial attacks can use different methods to generate or apply the malicious inputs. Some common methods are:

- **Gradient-based methods**: These methods use the gradient information of the model to find the optimal perturbations that can cause the desired output change. For example, the Fast Gradient Sign Method (FGSM) adds a small perturbation in the direction of the sign of the gradient to each input pixel or sample (Goodfellow et al., 2015).
- **Optimization-based methods**: These methods use optimization techniques such as genetic algorithms or evolutionary strategies to find the optimal perturbations that can cause the desired output change. For example, the Carlini and Wagner (CW) method uses an iterative optimization algorithm to minimize a loss function that incorporates both the model's output and the perturbation magnitude (Carlini and Wagner, 2017).
- **Score-based methods**: These methods use the score or confidence information of the model to find the optimal perturbations that can cause the desired output change. For example, the Jacobian-based Saliency Map Attack (JSMA) computes a saliency map that indicates how each input pixel or sample affects the model's output score, and then modifies the most salient ones (Papernot et al., 2016).
- **Query-based methods**: These methods use queries to access the model's output for different inputs and use feedback or reinforcement learning techniques to find
the optimal perturbations that can cause the desired output change. For example, the Zeroth Order Optimization (ZOO) method uses gradient estimation techniques based on queries to approximate gradient-based methods without accessing the model's gradient (Chen et al., 2017).

## Scenarios of Adversarial Attacks

Adversarial attacks can occur in different scenarios depending on the attacker's access and knowledge of the model and its environment. Some common scenarios are:

- **White-box attacks**: The attacker has full access and knowledge of the model's architecture, parameters, training data, and gradient information. The attacker can generate adversarial inputs offline and apply them online.
- **Black-box attacks**: The attacker has no access or knowledge of the model's architecture, parameters, training data, and gradient information. The attacker can only query
the model's output for different inputs. The attacker can generate adversarial inputs online using query-based methods or offline using transfer learning techniques.
- **Grey-box attacks**: The attacker has partial access or knowledge of some aspects