import struct
import os
import math
from PIL import Image


def clamp(value, min, max):
    if value < min:
        value = min
    if (value > max):
        value = max
    return value


def MakePalette():

    multiply = (209, 197, 174)
    pal = []

    palsize = 15
    for i in xrange(palsize):
        x = 255 - (i) * 12

        x = clamp(x, 0, 255)
        print(x)

        x = x + 1
        //进行正片叠底


        r = math.ceil((float(x) / float(255)) *
                      (float(multiply[0]) / float(255)) * float(255))
        g = math.ceil((float(x) / float(255)) *
                      (float(multiply[1]) / float(255)) * float(255))
        b = math.ceil((float(x) / float(255)) *
                      (float(multiply[2]) / float(255)) * float(255))

        r = int(clamp(r, 0, 255))
        g = int(clamp(g, 0, 255))
        b = int(clamp(b, 0, 255))
        pal.append((r, g, b, 255))
    pal.append((0, 0, 0, 0))
    return pal


def CreateBinary(pal):
    length = len(pal)
    dst = open("palette.bap", "wb")
    dst.write('\x62\x61\x70\x00')
    dst.write(struct.pack("I", 1))
    dst.write(struct.pack("I", 0x10))
    dst.write(struct.pack("I", 0))

    dst.seek(0x10)
    dstpng = Image.new("RGBA", (16, 1))
    x = 0
    for v in pal:
        dst.write(chr(v[0]))
        dst.write(chr(v[1]))
        dst.write(chr(v[2]))
        dst.write(chr(v[3]))
        dstpng.putpixel((x, 0), (v[0], v[1], v[2], v[3]))
        x += 1
    dst.close()
    dstpng.save("palette_aseprite.png")

b = MakePalette()
CreateBinary(b)
