var app = app || {};
(function () {

    $.extend(app, {
        consts: {
            maxProfilPictureBytesUserFriendlyValue: 5,
            maxFileBytesUserFriendlyValue: 5,
            grid: {
                defaultPageSize: 10,
                defaultPageSizes: [10, 20, 50, 100]
            },
            userManagement: {
                defaultAdminUserName: 'admin'
            },
            contentTypes: {
                formUrlencoded: 'application/x-www-form-urlencoded; charset=UTF-8'
            },
            friendshipState: {
                accepted: 1,
                blocked: 2
            },
            tieuChiDanhGia: {
                phanNhom: [
                    { id: 1, text: 'THONG_TIN_CHUNG' },
                    { id: 2, text: 'NHAN_THUC_SO' },
                    { id: 3, text: 'THE_CHE_SO' },
                    { id: 4, text: 'HA_TANG_SO' },
                    { id: 5, text: 'NHAN_LUC_SO' },
                    { id: 6, text: 'AN_TOAN_THONG_TIN_MANG' },
                    { id: 7, text: 'HOAT_DONG_CHINH_QUYEN_SO' },
                    { id: 8, text: 'HOAT_DONG_KINH_TE_SO' },
                    { id: 9, text: 'HOAT_DONG_XA_HOI_SO' },
                    { id: 10, text: 'DO_THI_THONG_MINH' },
                    { id: 11, text: 'THONG_TIN_VA_DU_LIEU_SO' },
                ],
            }
        },
        constsSoHoa: {
            maxProfilPictureBytesUserFriendlyValue: 5,
            maxFileBytesUserFriendlyValue: 500,
            grid: {
                defaultPageSize: 10,
                defaultPageSizes: [10, 20, 50, 100]
            },
            userManagement: {
                defaultAdminUserName: 'admin'
            },
            contentTypes: {
                formUrlencoded: 'application/x-www-form-urlencoded; charset=UTF-8'
            },
            friendshipState: {
                accepted: 1,
                blocked: 2
            },
            tieuChiDanhGia: {
                phanNhom: [
                    { id: 1, text: 'THONG_TIN_CHUNG' },
                    { id: 2, text: 'NHAN_THUC_SO' },
                    { id: 3, text: 'THE_CHE_SO' },
                    { id: 4, text: 'HA_TANG_SO' },
                    { id: 5, text: 'NHAN_LUC_SO' },
                    { id: 6, text: 'AN_TOAN_THONG_TIN_MANG' },
                    { id: 7, text: 'HOAT_DONG_CHINH_QUYEN_SO' },
                    { id: 8, text: 'HOAT_DONG_KINH_TE_SO' },
                    { id: 9, text: 'HOAT_DONG_XA_HOI_SO' },
                    { id: 10, text: 'DO_THI_THONG_MINH' },
                    { id: 11, text: 'THONG_TIN_VA_DU_LIEU_SO' },
                ],
            }
        },
        typeDuyetHoSoConst:{
            quanLyDuyet: 1,
            chanhVanPhongDuyet: 2
        },
        trangThaiDuyetHoSoConst:{
            dangChoDuyet:1,
            daDuyet:2
        }
    });

})();