using System;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Decision
{
    public class Satd : IDistortionMetric
    {
        private readonly ITransform transform;

        public Satd(ITransform transform)
        {
            this.transform = transform;
        }

        public int GetDistortion16X16(int[][] orig, int[][] pred)
        {
            var satd = 0;
            var m0 = new int[4][][][];
            for (var k = 0; k < m0.Length; k++)
            {
                m0[k] = new int[4][][];
                for (var l = 0; l < m0[k].Length; l++)
                {
                    m0[k][l] = new int[4][];
                    for (var m = 0; m < m0[k][l].Length; m++)
                        m0[k][l][m] = new int[4];
                }
            }

            var m4 = new int[4][];
            for (var k = 0; k < m4.Length; k++)
                m4[k] = new int[4];

            var m7 = new int[4][];
            for (var k = 0; k < m7.Length; k++)
                m7[k] = new int[4];

            for (var j = 0; j < 16; j++)
                for (var i = 0; i < 16; i++)
                    m0[j >> 2][i >> 2][j & 0x03][i & 0x03] = orig[j][i] - pred[j][i];

            for (var jj = 0; jj < 4; jj++)
            {
                for (var ii = 0; ii < 4; ii++)
                {
                    m7 = m0[jj][ii];
                    transform.Hadamard4X4(m7, m7);
                    for (var j = 0; j < 4; j++)
                        for (var i = 0; i < 4; i++)
                            if ((i + j) != 0)
                                satd += Math.Abs(m7[j][i]);
                }
            }

            for (var j = 0; j < 4; j++)
                for (var i = 0; i < 4; i++)
                    m4[j][i] = (m0[j][i][0][0] >> 1);

            transform.Hadamard4X4(m4, m4);

            for (var j = 0; j < 4; j++)
                for (var i = 0; i < 4; i++)
                    satd += Math.Abs(m4[j][i]);

            return satd;
        }

        public int GetDistortion4X4(int[][] orig, int[][] pred, int posY, int posX)
        {
            var satd = 0;
            var diff = new int[4][];
            for (var k = 0; k < diff.Length; k++)
                diff[k] = new int[4];

            var d = new int[16];
            var m = new int[16];

            for (var j = 0; j < 4; j++)
            {
                var jj = posY + j;
                for (var i = 0; i < 4; i++)
                {
                    var ii = posX + i;
                    diff[j][i] = orig[jj][ii] - pred[jj][ii];
                }
            }

            // TODO why the code bellow isn't equivalent to
            // transform.Hadamard4X4(diff, diff)?

            // hadamard transform
            m[0] = diff[0][0] + diff[3][0];
            m[1] = diff[0][1] + diff[3][1];
            m[2] = diff[0][2] + diff[3][2];
            m[3] = diff[0][3] + diff[3][3];
            m[4] = diff[1][0] + diff[2][0];
            m[5] = diff[1][1] + diff[2][1];
            m[6] = diff[1][2] + diff[2][2];
            m[7] = diff[1][3] + diff[2][3];
            m[8] = diff[1][0] - diff[2][0];
            m[9] = diff[1][1] - diff[2][1];
            m[10] = diff[1][2] - diff[2][2];
            m[11] = diff[1][3] - diff[2][3];
            m[12] = diff[0][0] - diff[3][0];
            m[13] = diff[0][1] - diff[3][1];
            m[14] = diff[0][2] - diff[3][2];
            m[15] = diff[0][3] - diff[3][3];

            d[0] = m[0] + m[4];
            d[1] = m[1] + m[5];
            d[2] = m[2] + m[6];
            d[3] = m[3] + m[7];
            d[4] = m[8] + m[12];
            d[5] = m[9] + m[13];
            d[6] = m[10] + m[14];
            d[7] = m[11] + m[15];
            d[8] = m[0] - m[4];
            d[9] = m[1] - m[5];
            d[10] = m[2] - m[6];
            d[11] = m[3] - m[7];
            d[12] = m[12] - m[8];
            d[13] = m[13] - m[9];
            d[14] = m[14] - m[10];
            d[15] = m[15] - m[11];

            m[0] = d[0] + d[3];
            m[1] = d[1] + d[2];
            m[2] = d[1] - d[2];
            m[3] = d[0] - d[3];
            m[4] = d[4] + d[7];
            m[5] = d[5] + d[6];
            m[6] = d[5] - d[6];
            m[7] = d[4] - d[7];
            m[8] = d[8] + d[11];
            m[9] = d[9] + d[10];
            m[10] = d[9] - d[10];
            m[11] = d[8] - d[11];
            m[12] = d[12] + d[15];
            m[13] = d[13] + d[14];
            m[14] = d[13] - d[14];
            m[15] = d[12] - d[15];

            d[0] = m[0] + m[1];
            d[1] = m[0] - m[1];
            d[2] = m[2] + m[3];
            d[3] = m[3] - m[2];
            d[4] = m[4] + m[5];
            d[5] = m[4] - m[5];
            d[6] = m[6] + m[7];
            d[7] = m[7] - m[6];
            d[8] = m[8] + m[9];
            d[9] = m[8] - m[9];
            d[10] = m[10] + m[11];
            d[11] = m[11] - m[10];
            d[12] = m[12] + m[13];
            d[13] = m[12] - m[13];
            d[14] = m[14] + m[15];
            d[15] = m[15] - m[14];

            for (var k = 0; k < 16; ++k)
                satd += Math.Abs(d[k]);

            return ((satd + 1) >> 1);
        }
    }
}