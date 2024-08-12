﻿// https://github.com/nesrak1/AssetStudio/tree/switch-tex-deswizzle

using SixLabors.ImageSharp;
using System;

namespace AssetStudio
{
    public class Texture2DSwitchDeswizzler
    {
        // referring to block here as a compressed texture block, not a gob one
        const int GOB_X_TEXEL_COUNT = 4;
        const int GOB_Y_TEXEL_COUNT = 8;
        const int TEXEL_BYTE_SIZE = 16;
        const int BLOCKS_IN_GOB = GOB_X_TEXEL_COUNT * GOB_Y_TEXEL_COUNT;
        static readonly int[] GOB_X_POSES = {
            0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3
        };
        static readonly int[] GOB_Y_POSES = {
            0, 1, 0, 1, 2, 3, 2, 3, 4, 5, 4, 5, 6, 7, 6, 7, 0, 1, 0, 1, 2, 3, 2, 3, 4, 5, 4, 5, 6, 7, 6, 7
        };

        /*
        sector:
        A
        B
         
        gob (made of sectors):
        ABIJ
        CDKL
        EFMN
        GHOP

        gob blocks (example with height 2):
        ACEGIK... from left to right of image
        BDFHJL...
        --------- start new row of blocks
        MOQSUW...
        NPRTVX...
        */

        private static int CeilDivide(int a, int b)
        {
            return (a + b - 1) / b;
        }

        internal static void Unswizzle(byte[] data, Size imageSize, Size blockSize, int gobsPerBlock, byte[] newData)
        {
            int width = imageSize.Width;
            int height = imageSize.Height;

            int blockCountX = CeilDivide(width, blockSize.Width);
            int blockCountY = CeilDivide(height, blockSize.Height);

            int gobCountX = blockCountX / GOB_X_TEXEL_COUNT;
            int gobCountY = blockCountY / GOB_Y_TEXEL_COUNT;

            int srcPos = 0;
            for (int i = 0; i < gobCountY / gobsPerBlock; i++)
            {
                for (int j = 0; j < gobCountX; j++)
                {
                    for (int k = 0; k < gobsPerBlock; k++)
                    {
                        for (int l = 0; l < BLOCKS_IN_GOB; l++)
                        {
                            int gobX = GOB_X_POSES[l];
                            int gobY = GOB_Y_POSES[l];
                            int gobDstX = j * GOB_X_TEXEL_COUNT + gobX;
                            int gobDstY = (i * gobsPerBlock + k) * GOB_Y_TEXEL_COUNT + gobY;
                            int gobDstLinPos = gobDstY * blockCountX * TEXEL_BYTE_SIZE + gobDstX * TEXEL_BYTE_SIZE;

                            Buffer.BlockCopy(data, srcPos, newData, gobDstLinPos, TEXEL_BYTE_SIZE);

                            srcPos += TEXEL_BYTE_SIZE;
                        }
                    }
                }
            }
        }

        //this should be the amount of pixels that can fit 16 bytes
        internal static Size GetTextureFormatBlockSize(TextureFormat m_TextureFormat)
        {
            switch (m_TextureFormat)
            {
                case TextureFormat.Alpha8: return new Size(16, 1); // 1 byte per pixel
                case TextureFormat.ARGB4444: return new Size(8, 1); // 2 bytes per pixel
                case TextureFormat.RGBA32: return new Size(4, 1); // 4 bytes per pixel
                case TextureFormat.ARGB32: return new Size(4, 1); // 4 bytes per pixel
                case TextureFormat.ARGBFloat: return new Size(1, 1); // 16 bytes per pixel (?)
                case TextureFormat.RGB565: return new Size(8, 1); // 2 bytes per pixel
                case TextureFormat.R16: return new Size(8, 1); // 2 bytes per pixel
                case TextureFormat.DXT1: return new Size(8, 4); // 8 bytes per 4x4=16 pixels
                case TextureFormat.DXT5: return new Size(4, 4); // 16 bytes per 4x4=16 pixels
                case TextureFormat.RGBA4444: return new Size(8, 1); // 2 bytes per pixel
                case TextureFormat.BGRA32: return new Size(4, 1); // 4 bytes per pixel
                case TextureFormat.BC6H: return new Size(4, 4); // 16 bytes per 4x4=16 pixels
                case TextureFormat.BC7: return new Size(4, 4); // 16 bytes per 4x4=16 pixels
                case TextureFormat.BC4: return new Size(8, 4); // 8 bytes per 4x4=16 pixels
                case TextureFormat.BC5: return new Size(4, 4); // 16 bytes per 4x4=16 pixels
                case TextureFormat.ASTC_RGB_4x4: return new Size(4, 4); // 16 bytes per 4x4=16 pixels
                case TextureFormat.ASTC_RGB_5x5: return new Size(5, 5); // 16 bytes per 5x5=25 pixels
                case TextureFormat.ASTC_RGB_6x6: return new Size(6, 6); // 16 bytes per 6x6=36 pixels
                case TextureFormat.ASTC_RGB_8x8: return new Size(8, 8); // 16 bytes per 8x8=64 pixels
                case TextureFormat.ASTC_RGB_10x10: return new Size(10, 10); // 16 bytes per 10x10=100 pixels
                case TextureFormat.ASTC_RGB_12x12: return new Size(12, 12); // 16 bytes per 12x12=144 pixels
                case TextureFormat.ASTC_RGBA_4x4: return new Size(4, 4); // 16 bytes per 4x4=16 pixels
                case TextureFormat.ASTC_RGBA_5x5: return new Size(5, 5); // 16 bytes per 5x5=25 pixels
                case TextureFormat.ASTC_RGBA_6x6: return new Size(6, 6); // 16 bytes per 6x6=36 pixels
                case TextureFormat.ASTC_RGBA_8x8: return new Size(8, 8); // 16 bytes per 8x8=64 pixels
                case TextureFormat.ASTC_RGBA_10x10: return new Size(10, 10); // 16 bytes per 10x10=100 pixels
                case TextureFormat.ASTC_RGBA_12x12: return new Size(12, 12); // 16 bytes per 12x12=144 pixels
                case TextureFormat.RG16: return new Size(8, 1); // 2 bytes per pixel
                case TextureFormat.R8: return new Size(16, 1); // 1 byte per pixel
                default: throw new NotImplementedException();
            };
        }

        internal static Size GetPaddedTextureSize(int width, int height, int blockWidth, int blockHeight, int gobsPerBlock)
        {
            width = CeilDivide(width, blockWidth * GOB_X_TEXEL_COUNT) * blockWidth * GOB_X_TEXEL_COUNT;
            height = CeilDivide(height, blockHeight * GOB_Y_TEXEL_COUNT * gobsPerBlock) * blockHeight * GOB_Y_TEXEL_COUNT * gobsPerBlock;
            return new Size(width, height);
        }
    }
}
