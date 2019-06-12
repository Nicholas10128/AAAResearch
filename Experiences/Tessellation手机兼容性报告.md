# Tessellation手机兼容性报告

作者：无聊

---
## 简介
&emsp;&emsp;Tessellation细分曲面技术是由ATI开发，微软采纳后将其加入DirectX 11，成为DirectX 11的组成部分之一。现在的光栅化图形渲染技术的核心是绘制大量三角形来组成3D模型，而Tessellation技术就是利用GPU硬件加速，将现有3D模型的三角形拆分得更细小、更细致，也就是大大增加三角形数量，使得渲染对象的表面和边缘更平滑、更精细。

&emsp;&emsp;为获取其在手机设备上的支持情况，制作使用Tessellation技术的雪地凹陷效果Demo，并做如下测试与分析。

## Android
&emsp;&emsp;利用WeTest测试237台手机，返回201台手机信息。具体测试信息如下：

        OpenGL ES 3.2	ShaderModel 5.0	112台
        OpenGL ES 3.1	ShaderModel 5.0	50台
        OpenGL ES 3.1	ShaderModel 4.5	16台
        OpenGL ES 3.0	ShaderModel 3.5	22台
        OpenGL ES 2.0	ShaderModel 3.0	1台


&emsp;&emsp;利用WeTest测试Top100常用手机，返回99台手机信息。具体测试信息如下：

        OpenGL ES 3.2	ShaderModel 5.0	58台
        OpenGL ES 3.1	ShaderModel 5.0	22台
        OpenGL ES 3.1	ShaderModel 4.5	10台
        OpenGL ES 3.0	ShaderModel 3.5	9台


&emsp;&emsp;根据Android官方开发文档，从Android5.0开始支持OpenGL ES 3.1和 Android extension Pack,信息来源如下:
https://developer.android.google.cn/about/versions/android-5.0

&emsp;&emsp;在所有测试的Android手机中，只有<font size=4>```Shader Model 5.0的设备支持Tessellation```</font>,可以运行雪地Demo。


## iOS

&emsp;&emsp;iPhone所使用的图形API为Metal，其Tessellation功能于iOS 10加入：
https://developer.apple.com/library/archive/releasenotes/General/WhatsNewIniOS/Articles/iOS10.html#//apple_ref/doc/uid/TP40017084-SW1

&emsp;&emsp;经测试，雪地demo在iPhone 6（iOS 10，显存256M）上不能运行；在iPhone 6s（iOS 10，显存512M）上可以运行。iPhone 6s在发售时默认系统为iOS 9。
因此，<font size=4>```Tessellation技术在iOS上运行要求：硬件最低为iPhone 6s (A9)，系统软件最低为iOS 10```</font>。

## 总结
1.<font size=4>```Tessellation技术在Android上运行要求：Shader Model 5.0的设备（OpenGL ES 3.1及以上设备大部分满足此条件，少数3.1的设备不满足）```</font>

2.<font size=4>```Tessellation技术在iOS上运行要求：硬件最低为iPhone 6s (A9)，系统软件最低为iOS 10```</font>
