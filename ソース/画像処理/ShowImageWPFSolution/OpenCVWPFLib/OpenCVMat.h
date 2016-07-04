#pragma once

namespace OpenCVWPFLib {

	public class OpenCVMat
	{
	public:
		OpenCVMat();	// コンストラクタ
		~OpenCVMat();	// デストラクタ

						// 画像ファイルの読み込み
		void load(std::string strPath);

		// 幅ピクセル
		int getWidth();
		// 高さピクセル
		int getHeight();
		// チャンネル数
		int getChannels();
		// １行のバイト数
		int getStride();
		// 画像バッファのポインタ
		unsigned char* getDataPtr();

	private:
		// OpenCVのMatオブジェクト変数
		cv::Mat m_mat;
	};
}