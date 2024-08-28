(function ($) {
    app.modals.CreateOrEditChiTietThuHoiModal = function () {

        var _chiTietThuHoiesService = abp.services.app.chiTietThuHoies;

        var _modalManager;
        var _$chiTietThuHoiInformationForm = null;

        var _ChiTietThuHoidanhMucThuHoiLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThuHoies/DanhMucThuHoiLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThuHoies/_ChiTietThuHoiDanhMucThuHoiLookupTableModal.js',
            modalClass: 'DanhMucThuHoiLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chiTietThuHoiInformationForm = _modalManager.getModal().find('form[name=ChiTietThuHoiInformationsForm]');
            _$chiTietThuHoiInformationForm.validate();
        };
        
        $('#OpenDanhMucThuHoiLookupTableButton').click(function () {

            var chiTietThuHoi = _$chiTietThuHoiInformationForm.serializeFormToObject();

            _ChiTietThuHoidanhMucThuHoiLookupTableModal.open({
                id: chiTietThuHoi.danhMucThuHoiId,
                displayName: chiTietThuHoi.danhMucThuHoiTen
            }, function (data) {
                _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiTen]').val(data.displayName);
                _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiId]').val(data.id);
            });
        });

        $('#ClearDanhMucThuHoiTenButton').click(function () {
            _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiTen]').val('');
            _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiId]').val('');
        });

        //inputmask
        $('.inputMask').inputmask();

        var thayDoiDu = function(){
           var du1 = $("#ChiTietThuHoi_Du1").inputmask('unmaskedvalue');
           var du2 = $("#ChiTietThuHoi_Du2").inputmask('unmaskedvalue');
           var du3 = $("#ChiTietThuHoi_Du3").inputmask('unmaskedvalue');
           var du4 = $("#ChiTietThuHoi_Du4").inputmask('unmaskedvalue');
           var du5 = $("#ChiTietThuHoi_Du5").inputmask('unmaskedvalue');
           var du6 = $("#ChiTietThuHoi_Du6").inputmask('unmaskedvalue');
           var du7 = $("#ChiTietThuHoi_Du7").inputmask('unmaskedvalue');
           var du8 = $("#ChiTietThuHoi_Du8").inputmask('unmaskedvalue');
           var du9 = $("#ChiTietThuHoi_Du9").inputmask('unmaskedvalue');
           var du10 = $("#ChiTietThuHoi_Du10").inputmask('unmaskedvalue');
           var du11 = $("#ChiTietThuHoi_Du11").inputmask('unmaskedvalue');
           var du12 = $("#ChiTietThuHoi_Du12").inputmask('unmaskedvalue');

           var tongDu = parseFloat(du1) + parseFloat(du2) + parseFloat(du3) + parseFloat(du4) + parseFloat(du5) + parseFloat(du6) +
               parseFloat(du7) + parseFloat(du8) + parseFloat(du9) + parseFloat(du10) + parseFloat(du11) + parseFloat(du12);
            $("#ChiTietThuHoi_TongDu").val(tongDu).trigger('input');
        };
        
        //Thay đổi tổng dư
        $("#ChiTietThuHoi_Du1, #ChiTietThuHoi_Du2, #ChiTietThuHoi_Du3, #ChiTietThuHoi_Du4, #ChiTietThuHoi_Du5, #ChiTietThuHoi_Du6, #ChiTietThuHoi_Du7, #ChiTietThuHoi_Du8, #ChiTietThuHoi_Du9, #ChiTietThuHoi_Du10, #ChiTietThuHoi_Du11, #ChiTietThuHoi_Du12").on("input", function () {
            thayDoiDu();
        });

        var thayDoiThu = function(){
            var thu1 = $("#ChiTietThuHoi_Thu1").inputmask('unmaskedvalue');
            var thu2 = $("#ChiTietThuHoi_Thu2").inputmask('unmaskedvalue');
            var thu3 = $("#ChiTietThuHoi_Thu3").inputmask('unmaskedvalue');
            var thu4 = $("#ChiTietThuHoi_Thu4").inputmask('unmaskedvalue');
            var thu5 = $("#ChiTietThuHoi_Thu5").inputmask('unmaskedvalue');
            var thu6 = $("#ChiTietThuHoi_Thu6").inputmask('unmaskedvalue');
            var thu7 = $("#ChiTietThuHoi_Thu7").inputmask('unmaskedvalue');
            var thu8 = $("#ChiTietThuHoi_Thu8").inputmask('unmaskedvalue');
            var thu9 = $("#ChiTietThuHoi_Thu9").inputmask('unmaskedvalue');
            var thu10 = $("#ChiTietThuHoi_Thu10").inputmask('unmaskedvalue');
            var thu11 = $("#ChiTietThuHoi_Thu11").inputmask('unmaskedvalue');
            var thu12 = $("#ChiTietThuHoi_Thu12").inputmask('unmaskedvalue');

            var tongThu = parseFloat(thu1) + parseFloat(thu2) + parseFloat(thu3) + parseFloat(thu4) + parseFloat(thu5) + parseFloat(thu6) +
                parseFloat(thu7) + parseFloat(thu8) + parseFloat(thu9) + parseFloat(thu10) + parseFloat(thu11) + parseFloat(thu12);
            $("#ChiTietThuHoi_TongThu").val(tongThu).trigger('input');
        };

        //Thay đổi tổng thu
        $("#ChiTietThuHoi_Thu1, #ChiTietThuHoi_Thu2, #ChiTietThuHoi_Thu3, #ChiTietThuHoi_Thu4, #ChiTietThuHoi_Thu5, #ChiTietThuHoi_Thu6, #ChiTietThuHoi_Thu7, #ChiTietThuHoi_Thu8, #ChiTietThuHoi_Thu9, #ChiTietThuHoi_Thu10, #ChiTietThuHoi_Thu11, #ChiTietThuHoi_Thu12").on("input", function () {
            thayDoiThu();
        });

        var thayDoiThucTe = function(){
            var thucTe1 = $("#ChiTietThuHoi_ThucTe1").inputmask('unmaskedvalue');
            var thucTe2 = $("#ChiTietThuHoi_ThucTe2").inputmask('unmaskedvalue');
            var thucTe3 = $("#ChiTietThuHoi_ThucTe3").inputmask('unmaskedvalue');
            var thucTe4 = $("#ChiTietThuHoi_ThucTe4").inputmask('unmaskedvalue');
            var thucTe5 = $("#ChiTietThuHoi_ThucTe5").inputmask('unmaskedvalue');
            var thucTe6 = $("#ChiTietThuHoi_ThucTe6").inputmask('unmaskedvalue');
            var thucTe7 = $("#ChiTietThuHoi_ThucTe7").inputmask('unmaskedvalue');
            var thucTe8 = $("#ChiTietThuHoi_ThucTe8").inputmask('unmaskedvalue');
            var thucTe9 = $("#ChiTietThuHoi_ThucTe9").inputmask('unmaskedvalue');
            var thucTe10 = $("#ChiTietThuHoi_ThucTe10").inputmask('unmaskedvalue');
            var thucTe11 = $("#ChiTietThuHoi_ThucTe11").inputmask('unmaskedvalue');
            var thucTe12 = $("#ChiTietThuHoi_ThucTe12").inputmask('unmaskedvalue');

            var tongThucTe = parseFloat(thucTe1) + parseFloat(thucTe2) + parseFloat(thucTe3) + parseFloat(thucTe4) + parseFloat(thucTe5) + parseFloat(thucTe6) +
                parseFloat(thucTe7) + parseFloat(thucTe8) + parseFloat(thucTe9) + parseFloat(thucTe10) + parseFloat(thucTe11) + parseFloat(thucTe12);
            $("#ChiTietThuHoi_TongThucTe").val(tongThucTe).trigger('input');
        };

        //Thay đổi tổng thực tế
        $("#ChiTietThuHoi_ThucTe1, #ChiTietThuHoi_ThucTe2, #ChiTietThuHoi_ThucTe3, #ChiTietThuHoi_ThucTe4, #ChiTietThuHoi_ThucTe5, #ChiTietThuHoi_ThucTe6, #ChiTietThuHoi_ThucTe7, #ChiTietThuHoi_ThucTe8, #ChiTietThuHoi_ThucTe9, #ChiTietThuHoi_ThucTe10, #ChiTietThuHoi_ThucTe11, #ChiTietThuHoi_ThucTe12").on("input", function () {
            thayDoiThucTe();
        });
        

        this.save = function () {
            if (!_$chiTietThuHoiInformationForm.valid()) {
                return;
            }
            if ($('#ChiTietThuHoi_DanhMucThuHoiId').prop('required') && $('#ChiTietThuHoi_DanhMucThuHoiId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DanhMucThuHoi')));
                return;
            }

            var chiTietThuHoi = _$chiTietThuHoiInformationForm.serializeFormToObject();
            chiTietThuHoi.du1 = $("#ChiTietThuHoi_Du1").inputmask('unmaskedvalue');
            chiTietThuHoi.du2 = $("#ChiTietThuHoi_Du2").inputmask('unmaskedvalue');
            chiTietThuHoi.du3 = $("#ChiTietThuHoi_Du3").inputmask('unmaskedvalue');
            chiTietThuHoi.du4 = $("#ChiTietThuHoi_Du4").inputmask('unmaskedvalue');
            chiTietThuHoi.du5 = $("#ChiTietThuHoi_Du5").inputmask('unmaskedvalue');
            chiTietThuHoi.du6 = $("#ChiTietThuHoi_Du6").inputmask('unmaskedvalue');
            chiTietThuHoi.du7 = $("#ChiTietThuHoi_Du7").inputmask('unmaskedvalue');
            chiTietThuHoi.du8 = $("#ChiTietThuHoi_Du8").inputmask('unmaskedvalue');
            chiTietThuHoi.du9 = $("#ChiTietThuHoi_Du9").inputmask('unmaskedvalue');
            chiTietThuHoi.du10 = $("#ChiTietThuHoi_Du10").inputmask('unmaskedvalue');
            chiTietThuHoi.du11 = $("#ChiTietThuHoi_Du11").inputmask('unmaskedvalue');
            chiTietThuHoi.du12 = $("#ChiTietThuHoi_Du12").inputmask('unmaskedvalue');
            chiTietThuHoi.tongDu = $("#ChiTietThuHoi_TongDu").inputmask('unmaskedvalue');
            
            chiTietThuHoi.thu1 = $("#ChiTietThuHoi_Thu1").inputmask('unmaskedvalue');
            chiTietThuHoi.thu2 = $("#ChiTietThuHoi_Thu2").inputmask('unmaskedvalue');
            chiTietThuHoi.thu3 = $("#ChiTietThuHoi_Thu3").inputmask('unmaskedvalue');
            chiTietThuHoi.thu4 = $("#ChiTietThuHoi_Thu4").inputmask('unmaskedvalue');
            chiTietThuHoi.thu5 = $("#ChiTietThuHoi_Thu5").inputmask('unmaskedvalue');
            chiTietThuHoi.thu6 = $("#ChiTietThuHoi_Thu6").inputmask('unmaskedvalue');
            chiTietThuHoi.thu7 = $("#ChiTietThuHoi_Thu7").inputmask('unmaskedvalue');
            chiTietThuHoi.thu8 = $("#ChiTietThuHoi_Thu8").inputmask('unmaskedvalue');
            chiTietThuHoi.thu9 = $("#ChiTietThuHoi_Thu9").inputmask('unmaskedvalue');
            chiTietThuHoi.thu10 = $("#ChiTietThuHoi_Thu10").inputmask('unmaskedvalue');
            chiTietThuHoi.thu11 = $("#ChiTietThuHoi_Thu11").inputmask('unmaskedvalue');
            chiTietThuHoi.thu12 = $("#ChiTietThuHoi_Thu12").inputmask('unmaskedvalue');
            chiTietThuHoi.tongThu = $("#ChiTietThuHoi_TongThu").inputmask('unmaskedvalue');

            chiTietThuHoi.thucTe1 = $("#ChiTietThuHoi_ThucTe1").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe2 = $("#ChiTietThuHoi_ThucTe2").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe3 = $("#ChiTietThuHoi_ThucTe3").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe4 = $("#ChiTietThuHoi_ThucTe4").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe5 = $("#ChiTietThuHoi_ThucTe5").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe6 = $("#ChiTietThuHoi_ThucTe6").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe7 = $("#ChiTietThuHoi_ThucTe7").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe8 = $("#ChiTietThuHoi_ThucTe8").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe9 = $("#ChiTietThuHoi_ThucTe9").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe10 = $("#ChiTietThuHoi_ThucTe10").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe11 = $("#ChiTietThuHoi_ThucTe11").inputmask('unmaskedvalue');
            chiTietThuHoi.thucTe12 = $("#ChiTietThuHoi_ThucTe12").inputmask('unmaskedvalue');
            chiTietThuHoi.tongThucTe = $("#ChiTietThuHoi_TongThucTe").inputmask('unmaskedvalue');
            
            _modalManager.setBusy(true);
            _chiTietThuHoiesService.createOrEdit(
                chiTietThuHoi
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditChiTietThuHoiModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);