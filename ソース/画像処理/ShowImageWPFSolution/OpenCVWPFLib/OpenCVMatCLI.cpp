// これは メイン DLL ファイルです。

#include "stdafx.h"

#include "OpenCVMatCLI.h"

using namespace System::Windows;
using namespace System::Windows::Media;
using namespace Runtime::InteropServices;

using namespace OpenCVWPFLib;

// ヘルパー関数
// String(UNICODE)からANSI(ダブルバイト)文字列へ変換
void StringToAnsi(String^ inStr, std::string& outStr)
{
	const char* chars =
		(const char*)
		(Marshal::StringToHGlobalAnsi(inStr)).ToPointer();
	outStr = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

// コンストラクタ
OpenCVMatCLI::OpenCVMatCLI()
{
	m_pOpenCVMat = new OpenCVMat();
}

// デストラクタ
OpenCVMatCLI::~OpenCVMatCLI()
{
	this->!OpenCVMatCLI();
}

// ファイナライザ
OpenCVMatCLI::!OpenCVMatCLI()
{
	if (m_pOpenCVMat != NULL)
	{
		delete m_pOpenCVMat;
	}
}

// 画像ファイルの読み込み
void OpenCVMatCLI::load(String^ path)
{
	if (m_pOpenCVMat != NULL)
	{
		// String^からstd::strinegへ
		std::string strFilename;
		StringToAnsi(path, strFilename);

		m_pOpenCVMat->load(strFilename);
	}
}

// MatからBItmapSourceへ変換
BitmapSource^ OpenCVMatCLI::getBitmapSource()
{
	WriteableBitmap^ writeableBitmap = nullptr;

	if (m_pOpenCVMat != NULL)
	{
		int width = m_pOpenCVMat->getWidth();
		int height = m_pOpenCVMat->getHeight();
		int channels = m_pOpenCVMat->getChannels();
		int stride = m_pOpenCVMat->getStride();
		int bufferSize = stride * height;
		unsigned char* pData = m_pOpenCVMat->getDataPtr();

		if ((width != -1) && (height != -1) &&
			(channels != -1) && (stride != -1) &&
			(pData != NULL))
		{
			// imreadで読み込まれた画像は24bitフルカラーなので、
			// 常に3chになる
			// 念のため3ch以外はnullを返す
			if (channels == 3)
			{
				writeableBitmap =
					gcnew WriteableBitmap(width, height, 96, 96, PixelFormats::Bgr24, nullptr);

				Int32Rect rect(0, 0, width, height);
				writeableBitmap->WritePixels(rect, IntPtr(pData), bufferSize, stride);
			}
		}
	}

	return writeableBitmap;
}

