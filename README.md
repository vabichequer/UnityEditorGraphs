# Unity Editor Graphs!

This is a repository for whoever may need line plots within Unity's editor. Since I'm not able to continuously maintain it, I will keep updating this whenever I can and add things slowly. Whoever wants to is obvjously welcome to contribute. This is mainly focused on an exploration of the limits of UI Toolkit.

# Roadmap

No map! I will add things as needed/requested, but most importantly wanted.

# Usage

Simply open the graphing tool through the Window > Graphs menu. When it is opened, you can drag a GameObject into the Object To Analyze box and each of its components will be exposed through reflection. Select the property through the dropdowns and that's it.
![Uploading image.png…]()

# Goals

The main idea behind this is to be a playground to explore UI Toolkit. Since UI Toolkit doesn't have native OpenGL support, I've injected IMGUI code through a IMGUI contained within UI Toolkit and have built a small library to draw shapes.

# Rules

No TextMeshPro should be used. This package will be discontinued, so either focus on using a bitmap font or the text drawing library I've written.
