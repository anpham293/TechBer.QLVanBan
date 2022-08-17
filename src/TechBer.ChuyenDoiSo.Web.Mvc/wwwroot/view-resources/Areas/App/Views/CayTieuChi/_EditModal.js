(function() {
    app.modals.EditCayTieuChiModal = function () {

        var _modalManager;
        var _tieuChiDanhGiasAppService = abp.services.app.tieuChiDanhGias;
        var _$form = null;

        this.init = function(modalManager) {
            _modalManager = modalManager;

            _$form = _modalManager.getModal().find('form[name=CayTieuChiDanhGiaForm]');
            _$form.validate({ ignore: ":hidden:not(#TieuChiDanhGia_PhuongThucDanhGia),.note-editable" });
        };

        $('#TieuChiDanhGia_PhuongThucDanhGia').summernote({
            height: 150
        })

        $("#TieuChiDanhGia_PhanNhom").select2({
            allowClear: true,
            placeholder: "Chọn phân nhóm...",
            data: app.consts.tieuChiDanhGia.phanNhom,
            width: '100%',
            minimumResultsForSearch: -1,
        });

        var select2Element = $('.kt-select2');
        select2Element.val($("#TieuChiDanhGia_PhanNhom").attr("value")).trigger('change');

        this.save = function() {
            if (!_$form.valid()) {
                return;
            }

            var cayTieuChiData = _$form.serializeFormToObject();

            console.log(cayTieuChiData);

            _modalManager.setBusy(true);
            _tieuChiDanhGiasAppService.createOrEditTieuChi(
                cayTieuChiData
            ).done(function(result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                _modalManager.setResult(result);
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})();