(function ($) {
    app.modals.CreateOrEditDanhMucThuHoiModal = function () {

        var _danhMucThuHoiesService = abp.services.app.danhMucThuHoies;

        var _modalManager;
        var _$danhMucThuHoiInformationForm = null;

		        var _DanhMucThuHoiduAnThuHoiLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DanhMucThuHoies/DuAnThuHoiLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DanhMucThuHoies/_DanhMucThuHoiDuAnThuHoiLookupTableModal.js',
            modalClass: 'DuAnThuHoiLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$danhMucThuHoiInformationForm = _modalManager.getModal().find('form[name=DanhMucThuHoiInformationsForm]');
            _$danhMucThuHoiInformationForm.validate();
        };

		          $('#OpenDuAnThuHoiLookupTableButton').click(function () {

            var danhMucThuHoi = _$danhMucThuHoiInformationForm.serializeFormToObject();

            _DanhMucThuHoiduAnThuHoiLookupTableModal.open({ id: danhMucThuHoi.duAnThuHoiId, displayName: danhMucThuHoi.duAnThuHoiMaDATH }, function (data) {
                _$danhMucThuHoiInformationForm.find('input[name=duAnThuHoiMaDATH]').val(data.displayName); 
                _$danhMucThuHoiInformationForm.find('input[name=duAnThuHoiId]').val(data.id); 
            });
        });
		
		$('#ClearDuAnThuHoiMaDATHButton').click(function () {
                _$danhMucThuHoiInformationForm.find('input[name=duAnThuHoiMaDATH]').val(''); 
                _$danhMucThuHoiInformationForm.find('input[name=duAnThuHoiId]').val(''); 
        });
		


        this.save = function () {
            if (!_$danhMucThuHoiInformationForm.valid()) {
                return;
            }
            if ($('#DanhMucThuHoi_DuAnThuHoiId').prop('required') && $('#DanhMucThuHoi_DuAnThuHoiId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DuAnThuHoi')));
                return;
            }

            var danhMucThuHoi = _$danhMucThuHoiInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _danhMucThuHoiesService.createOrEdit(
				danhMucThuHoi
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDanhMucThuHoiModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);