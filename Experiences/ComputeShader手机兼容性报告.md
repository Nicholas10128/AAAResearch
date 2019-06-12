# ComputeShader手机兼容性报告

作者：无聊

---
## 简介
&emsp;&emsp;Compute Shader是微软DirectX 11 API新加入的特性，在Compute Shader的帮助下，程序员可直接将GPU作为并行处理器加以利用，GPU将不仅具有3D渲染能力，也具有其他的运算能力，也就是我们说的GPGPU的概念和物理加速运算。多线程处理技术使游戏更好地利用系统的多个核心。故对其在手机上的支持情况做了如下测试与分析。

## Android
&emsp;&emsp;利用WeTest测试213台手机，返回201台手机信息。具体测试信息如下：

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


&emsp;&emsp;Android5.0开始支持Compute Shaders，来源如下：
https://developer.android.google.cn/about/versions/android-5.0


&emsp;&emsp;在所有测试的Android手机中，OpenGL ES 3.1以上的手机均使用的是Shader Model 4.5或Shader Model 5.0，在使用unity提供的API  SystemInfo.supportsComputeShaders在上述手机显示为true，但通过运行一段ComputeShader程序，在Shader Model4.5上运行结果却不符合预期。

&emsp;&emsp;综上所述，<font size=4>```Shader Model 5.0开始支持ComputeShader```</font>。但文档说OpenGL ES 3.1就已经支持，unity API返回结果也是如此，为什么Shader Model4.5不支持还有待探讨。


## iOS

&emsp;&emsp;根据苹果开发文档，从<font size=4>```iOS 9开始支持Compute Shaders```</font>：
https://developer.apple.com/library/archive/releasenotes/General/WhatsNewIniOS/Articles/iOS9.html#//apple_ref/doc/uid/TP40016198-SW1


&emsp;&emsp;iPhone所使用的图形API为Metal，ComputeShader在iphone6 ios10 系统已经支持（再早的iphone手机由于没有测试设备，没有分析）。
以上测试方法是通过两方面进行，一是使用unity提供的API SystemInfo.supportsComputeShaders在上述设备显示为true，二是通过运行一段ComputeShader程序，运行结果也符合预期。

## 总结

1.<font size=4>```ComputeShader技术在Android上运行要求：Shader Model 5.0的设备（OpenGL ES 3.1及以上设备大部分满足此条件，少数3.1的设备不满足）```</font>。

2.<font size=4>```ComputeShader技术在iOS上运行要求：系统软件最低为iOS 9（iPhone6之前的因缺乏设备未能测试，有需要的看官请自行测试）```</font>。
