(function() {
    app.modals.CreateCayTieuChiModal = function () {

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
            data: app.consts.tieuChiDanhGia.phanNhom,
            width: '100%',
            minimumResultsForSearch: -1,
        });

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
                _modalManager.setResult(result);
                _modalManager.close();
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})();