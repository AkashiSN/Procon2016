#include "stdafx.h"
#include "OpenCVMat.h"

using namespace OpenCVWPFLib;

// �R���X�g���N�^
OpenCVMat::OpenCVMat()
{
}

// �f�X�g���N�^
OpenCVMat::~OpenCVMat()
{
	// m_mat�̓|�C���^�łȂ��̂ŁA�N���X���Ŏ��Ƀ����[�X
	// �ȉ��͂���Ȃ�
	//if (m_mat.data != NULL)
	//{
	//	m_mat.release();
	//}
}

// �摜�t�@�C���̓ǂݍ���
void OpenCVMat::load(std::string strPath)
{
	//if (m_mat.data != NULL)
	//{
	//	m_mat.release();
	//}
	// =�ɂĐV����Mat�������́A�Â��f�[�^�̎Q�Ƃ��O����Ă���
	// ��������
	// �Q�Ƃ�������̎��̓f�[�^�̓����[�X
	// �Ȃ̂ŏ�͂���Ȃ�
	m_mat = cv::imread(strPath);
}

// ���s�N�Z��
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

// �����s�N�Z��
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

// �`�����l����
int OpenCVMat::getChannels()
{
	int channels = -1;

	if (m_mat.data != NULL)
	{
		channels = m_mat.channels();
	}

	return channels;
}

// �P�s�̃o�C�g��
int OpenCVMat::getStride()
{
	int stride = -1;

	if (m_mat.data != NULL)
	{
		stride = m_mat.step;
	}

	return stride;
}

// �摜�o�b�t�@�̃|�C���^
unsigned char* OpenCVMat::getDataPtr()
{
	return m_mat.data;
}
