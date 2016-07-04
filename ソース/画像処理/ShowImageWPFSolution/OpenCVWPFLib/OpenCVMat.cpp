#include "stdafx.h"
#include "OpenCVMat.h"

using namespace OpenCVWPFLib;

// コンストラクタ
OpenCVMat::OpenCVMat()
{
}

// デストラクタ
OpenCVMat::~OpenCVMat()
{
	// m_matはポインタでないので、クラス消滅時にリリース
	// 以下はいらない
	//if (m_mat.data != NULL)
	//{
	//	m_mat.release();
	//}
}

// 画像ファイルの読み込み
void OpenCVMat::load(std::string strPath)
{
	//if (m_mat.data != NULL)
	//{
	//	m_mat.release();
	//}
	// =にて新しいMatを代入時は、古いデータの参照が外されてから
	// 代入される
	// 参照が一つだけの時はデータはリリース
	// なので上はいらない
	m_mat = cv::imread(strPath);
}

// 幅ピクセル
int OpenCVMat::getWidth()
{
	int width = -1;

	if (m_mat.data != NULL)
	{
		cv::Size size = m_mat.size();
		width = size.width;
	}

	return width;
}

// 高さピクセル
int OpenCVMat::getHeight()
{
	int height = -1;

	if (m_mat.data != NULL)
	{
		cv::Size size = m_mat.size();
		height = size.height;
	}

	return height;
}

// チャンネル数
int OpenCVMat::getChannels()
{
	int channels = -1;

	if (m_mat.data != NULL)
	{
		channels = m_mat.channels();
	}

	return channels;
}

// １行のバイト数
int OpenCVMat::getStride()
{
	int stride = -1;

	if (m_mat.data != NULL)
	{
		stride = m_mat.step;
	}

	return stride;
}

// 画像バッファのポインタ
unsigned char* OpenCVMat::getDataPtr()
{
	return m_mat.data;
}
