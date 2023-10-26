/*
Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://cksource.com/ckfinder/license
*/

CKFinder.customConfig = function( config )
{
	// Define changes to default configuration here.
	// For the list of available options, check:
	// http://docs.cksource.com/ckfinder_2.x_api/symbols/CKFinder.config.html

	// Sample configuration options:
	//config.uiColor = '#BDE31E';
	//config.language = 'vi';
	//config.removePlugins = 'basket';

    // Thiết lập đường dẫn đến CKFinder
    config.basePath = '/ckfinder/';

    // Thiết lập đường dẫn đến trình duyệt tệp
    config.connectorPath = 'connector.aspx'; // Tùy thuộc vào cấu hình của bạn

    // Thiết lập các tùy chọn khác nếu cần
    config.skin = 'kama'; // Giao diện của CKFinder
    config.disableHelpButton = true; // Tắt nút trợ giúp

    // Cấu hình đường dẫn cho tải lên tệp
    config.uploadUrl = '~/wwwroot/Uploads'; // Tùy thuộc vào cấu hình của bạn

    // Cấu hình đường dẫn cho trình duyệt tệp
    config.browseUrl = '~/wwwroot/Browse'; // Tùy thuộc vào cấu hình của bạn

    // Thiết lập loại tệp cho phép
    config.allowedExtensions = 'jpg,jpeg,png,gif,pdf,doc,docx'; // Danh sách loại tệp cho phép

    // Thiết lập kích thước tối đa của tệp được tải lên (theo byte)
    config.maxSize = 1024 * 1024 * 10; // 10MB

    // Cấu hình loại tệp cho từng loại MIME (tùy chọn)
    config.fileTypeConfig = {
        'Image': {
            'extensions': 'jpg,jpeg,png,gif',
            'maxSize': 1024 * 1024 * 5 // 5MB
        },
        'PDF': {
            'extensions': 'pdf',
            'maxSize': 1024 * 1024 * 10 // 10MB
        },
        'Document': {
            'extensions': 'doc,docx',
            'maxSize': 1024 * 1024 * 5 // 5MB
        }
    };

    // Cấu hình danh sách các loại tệp được tạo ra (tùy thuộc vào cấu hình của bạn)
    config.typesConfig = {
        'Image': {
            'html': 'Image',
            'icons': ['file.png']
        },
        'PDF': {
            'html': 'PDF',
            'icons': ['file.png']
        },
        'Document': {
            'html': 'Document',
            'icons': ['file.png']
        }
    };

};
