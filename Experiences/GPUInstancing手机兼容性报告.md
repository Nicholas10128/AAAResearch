# GPU Instancing手机兼容性报告
---

## 简介

GPU Insancing是Unity 5.4加入的新功能，使用GPU Instancing可以批渲染绘制Mesh相同的物体，减少draw call。

## Android

利用WeTest测试252台手机是否支持GPU Instancing。具体测试信息如下：

    OpenGL ES 3.2	ShaderModel 5.0	 125台	  支持
    OpenGL ES 3.1	ShaderModel 5.0	 59台	   支持
    OpenGL ES 3.1	ShaderModel 4.5	 18台	   支持
    OpenGL ES 3.0	ShaderModel 3.5	 44台	   4台支持，40台不支持
    OpenGL ES 2.0	ShaderModel 3.0	 6台		不支持

根据Unity官方文档，OpenGL ES 3.0+开始支持GPU Instancing，来源如下：https://docs.unity3d.com/Manual/GPUInstancing.html

由于驱动程序问题，对于仅具有OpenGL ES 3.0的Adreno GPU的Android设备禁用了GPU实例支持。来源如下：https://unity3d.com/unity/beta/unity5.5.0b6

    Graphics: GPU Instancing: Added support for Android with OpenGL ES 3.0 or newer. Note however that GPU instancing support is disabled for Android devices that have the Adreno GPU with only OpenGL ES 3.0, because of driver issues.

测试根据Unity提供的接口SystemInfo.supportsInstancing检测机器是否支持GPU Instancing。根据测试结果，所有在OpenGL ES 3.1及以上的Android手机均支持GPU Instancing。OpenGL ES 2.0的手机均不支持，OpenGL ES 3.0的手机中40台不支持，其余4台支持。这4台支持的手机（华为PE-TL20，小米红米2A，OPPO R7,三星 NOTE3）均不是骁龙处理器。

综上所述，应该是<font size=4>```OpenGL ES 3.1开始完全支持GPU Instancing```</font>。

## iOS

Unity官方文档，<font size=4>```iOS的Metal均支持GPU Instancing```</font>：https://docs.unity3d.com/Manual/GPUInstancing.html

iPhone所使用的图形API为Metal，在2014年推出，从在Apple A7（iPhone 5s）与更新的处理器上。2018年苹果宣布，从iOS 12开始弃用OpenGL/CL，仅支持Metal。

## 总结

1.**GPU Insancing技术在Android上运行要求：OpenGL ES 3.1开始完全支持GPU Instancing（OpenGL ES 3.0高通芯片的手机不支持）**。

2.**GPU Insancing技术在iOS上运行要求：iOS的Metal均支持GPU Instancing。Metal从Apple A7（iPhone 5s）开始支持**。
