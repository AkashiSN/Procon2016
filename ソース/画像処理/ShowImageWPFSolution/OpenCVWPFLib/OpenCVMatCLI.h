// OpenCVWPFLib.h

#pragma once
#include "OpenCVMat.h"

using namespace System;
using namespace System::Windows::Media::Imaging;

namespace OpenCVWPFLib {

	public ref class OpenCVMatCLI
	{
		// TODO: このクラスの、ユーザーのメソッドをここに追加してください。
	public:
		OpenCVMatCLI(); // コンストラクタ
		~OpenCVMatCLI();    // デストラクタ
		!OpenCVMatCLI();    // ファイナライザ

							// 画像ファイルの読み込み
		void load(String^ path);

		// プロパティ
		// BitmapSourceオブジェクトの取得
		property BitmapSource^ Image
		{
			BitmapSource^ get()
			{
				return getBitmapSource();
			}
		}

	private:
		// OpenCVのMatを扱うクラス
		OpenCVMat *m_pOpenCVMat;

		// MatからBItmapへ変換メソッド
		BitmapSource^ getBitmapSource();
	};
}
