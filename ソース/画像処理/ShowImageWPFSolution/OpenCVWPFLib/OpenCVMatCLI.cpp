// ����� ���C�� DLL �t�@�C���ł��B

#include "stdafx.h"

#include "OpenCVMatCLI.h"

using namespace System::Windows;
using namespace System::Windows::Media;
using namespace Runtime::InteropServices;

using namespace OpenCVWPFLib;

// �w���p�[�֐�
// String(UNICODE)����ANSI(�_�u���o�C�g)������֕ϊ�
void StringToAnsi(String^ inStr, std::string& outStr)
{
	const char* chars =
		(const char*)
		(Marshal::StringToHGlobalAnsi(inStr)).ToPointer();
	outStr = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

// �R���X�g���N�^
OpenCVMatCLI::OpenCVMatCLI()
{
	m_pOpenCVMat = new OpenCVMat();
}

// �f�X�g���N�^
OpenCVMatCLI::~OpenCVMatCLI()
{
	this->!OpenCVMatCLI();
}

// �t�@�C�i���C�U
OpenCVMatCLI::!OpenCVMatCLI()
{
	if (m_pOpenCVMat != NULL)
	{
		delete m_pOpenCVMat;
	}
}

// �摜�t�@�C���̓ǂݍ���
void OpenCVMatCLI::load(String^ path)
{
	if (m_pOpenCVMat != NULL)
	{
		// String^����std::strineg��
		std::string strFilename;
		StringToAnsi(path, strFilename);

		m_pOpenCVMat->load(strFilename);
	}
}

// Mat����BItmapSource�֕ϊ�
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
			// imread�œǂݍ��܂ꂽ�摜��24bit�t���J���[�Ȃ̂ŁA
			// ���3ch�ɂȂ�
			// �O�̂���3ch�ȊO��null��Ԃ�
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

