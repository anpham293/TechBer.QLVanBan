(function ($) {
    app.modals.CreateOrEditChuyenHoSoGiayModal = function () {

        var _chuyenHoSoGiaiesService = abp.services.app.chuyenHoSoGiaies;

        var _modalManager;
        var _$chuyenHoSoGiayInformationForm = null;

		        var _ChuyenHoSoGiayvanBanDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenHoSoGiaies/VanBanDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChuyenHoSoGiaies/_ChuyenHoSoGiayVanBanDuAnLookupTableModal.js',
            modalClass: 'VanBanDuAnLookupTableModal'
        });        var _ChuyenHoSoGiayuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenHoSoGiaies/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChuyenHoSoGiaies/_ChuyenHoSoGiayUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chuyenHoSoGiayInformationForm = _modalManager.getModal().find('form[name=ChuyenHoSoGiayInformationsForm]');
            _$chuyenHoSoGiayInformationForm.validate();
        };

		          $('#OpenVanBanDuAnLookupTableButton').click(function () {

            var chuyenHoSoGiay = _$chuyenHoSoGiayInformationForm.serializeFormToObject();

            _ChuyenHoSoGiayvanBanDuAnLookupTableModal.open({ id: chuyenHoSoGiay.vanBanDuAnId, displayName: chuyenHoSoGiay.vanBanDuAnName }, function (data) {
                _$chuyenHoSoGiayInformationForm.find('input[name=vanBanDuAnName]').val(data.displayName); 
                _$chuyenHoSoGiayInformationForm.find('input[name=vanBanDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearVanBanDuAnNameButton').click(function () {
                _$chuyenHoSoGiayInformationForm.find('input[name=vanBanDuAnName]').val(''); 
                _$chuyenHoSoGiayInformationForm.find('input[name=vanBanDuAnId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var chuyenHoSoGiay = _$chuyenHoSoGiayInformationForm.serializeFormToObject();

            _ChuyenHoSoGiayuserLookupTableModal.open({ id: chuyenHoSoGiay.nguoiNhanId, displayName: chuyenHoSoGiay.userName }, function (data) {
                _$chuyenHoSoGiayInformationForm.find('input[name=userName]').val(data.displayName); 
                _$chuyenHoSoGiayInformationForm.find('input[name=nguoiNhanId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$chuyenHoSoGiayInformationForm.find('input[name=userName]').val(''); 
                _$chuyenHoSoGiayInformationForm.find('input[name=nguoiNhanId]').val(''); 
        });
		


        this.save = function () {
            if (!_$chuyenHoSoGiayInformationForm.valid()) {
                return;
            }
            if ($('#ChuyenHoSoGiay_VanBanDuAnId').prop('required') && $('#ChuyenHoSoGiay_VanBanDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('VanBanDuAn')));
                return;
            }
            if ($('#ChuyenHoSoGiay_NguoiNhanId').prop('required') && $('#ChuyenHoSoGiay_NguoiNhanId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var chuyenHoSoGiay = _$chuyenHoSoGiayInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _chuyenHoSoGiaiesService.createOrEdit(
				chuyenHoSoGiay
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditChuyenHoSoGiayModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);