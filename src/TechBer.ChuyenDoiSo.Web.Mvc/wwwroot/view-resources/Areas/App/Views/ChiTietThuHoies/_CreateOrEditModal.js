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

            _ChiTietThuHoidanhMucThuHoiLookupTableModal.open({ id: chiTietThuHoi.danhMucThuHoiId, displayName: chiTietThuHoi.danhMucThuHoiTen }, function (data) {
                _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiTen]').val(data.displayName); 
                _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiId]').val(data.id); 
            });
        });
		
		$('#ClearDanhMucThuHoiTenButton').click(function () {
                _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiTen]').val(''); 
                _$chiTietThuHoiInformationForm.find('input[name=danhMucThuHoiId]').val(''); 
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