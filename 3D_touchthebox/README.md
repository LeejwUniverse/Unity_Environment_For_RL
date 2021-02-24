# 3D Touch the box envrionment

- - -
#### Play example
<div align="center">
    <img src="https://github.com/LeejwUniverse/Unity_Environment_For_RL/blob/master/3D_touchthebox/etc/env_gif.gif" width="500">
</div>

## env info
* Agent type: Single
* State space: 21
|Type|Detail|Space|
|---|---|---|
|Agent position|x, y, z|3|
|Agent velocity|x, z|2|
|Box position|x, y, z|3 * 4|
|Box Color flag|0 or 1|4|
* Action space: 2 (Continuous)
* Reward
|Type|Detail|
|---|---|
|Touch the box|+1|
|Every steps|-0.01|
