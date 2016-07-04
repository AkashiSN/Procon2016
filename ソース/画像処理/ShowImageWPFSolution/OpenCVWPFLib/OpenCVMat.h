#pragma once

namespace OpenCVWPFLib {

	public class OpenCVMat
	{
	public:
		OpenCVMat();	// �R���X�g���N�^
		~OpenCVMat();	// �f�X�g���N�^

						// �摜�t�@�C���̓ǂݍ���
		void load(std::string strPath);

		// ���s�N�Z��
		int getWidth();
		// �����s�N�Z��
		int getHeight();
		// �`�����l����
		int getChannels();
		// �P�s�̃o�C�g��
		int getStride();
		// �摜�o�b�t�@�̃|�C���^
		unsigned char* getDataPtr();

	private:
		// OpenCV��Mat�I�u�W�F�N�g�ϐ�
		cv::Mat m_mat;
	};
}