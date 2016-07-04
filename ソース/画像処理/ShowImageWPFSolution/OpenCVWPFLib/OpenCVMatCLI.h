// OpenCVWPFLib.h

#pragma once
#include "OpenCVMat.h"

using namespace System;
using namespace System::Windows::Media::Imaging;

namespace OpenCVWPFLib {

	public ref class OpenCVMatCLI
	{
		// TODO: ���̃N���X�́A���[�U�[�̃��\�b�h�������ɒǉ����Ă��������B
	public:
		OpenCVMatCLI(); // �R���X�g���N�^
		~OpenCVMatCLI();    // �f�X�g���N�^
		!OpenCVMatCLI();    // �t�@�C�i���C�U

							// �摜�t�@�C���̓ǂݍ���
		void load(String^ path);

		// �v���p�e�B
		// BitmapSource�I�u�W�F�N�g�̎擾
		property BitmapSource^ Image
		{
			BitmapSource^ get()
			{
				return getBitmapSource();
			}
		}

	private:
		// OpenCV��Mat�������N���X
		OpenCVMat *m_pOpenCVMat;

		// Mat����BItmap�֕ϊ����\�b�h
		BitmapSource^ getBitmapSource();
	};
}
