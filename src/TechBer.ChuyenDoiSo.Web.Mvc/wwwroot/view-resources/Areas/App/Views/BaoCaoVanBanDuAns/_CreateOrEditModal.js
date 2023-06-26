(function ($) {
    app.modals.CreateOrEditBaoCaoVanBanDuAnModal = function () {

        var _baoCaoVanBanDuAnsService = abp.services.app.baoCaoVanBanDuAns;

        var _modalManager;
        var _$baoCaoVanBanDuAnInformationForm = null;

		        var _BaoCaoVanBanDuAnvanBanDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BaoCaoVanBanDuAns/VanBanDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BaoCaoVanBanDuAns/_BaoCaoVanBanDuAnVanBanDuAnLookupTableModal.js',
            modalClass: 'VanBanDuAnLookupTableModal'
        });        var _BaoCaoVanBanDuAnuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BaoCaoVanBanDuAns/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BaoCaoVanBanDuAns/_BaoCaoVanBanDuAnUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$baoCaoVanBanDuAnInformationForm = _modalManager.getModal().find('form[name=BaoCaoVanBanDuAnInformationsForm]');
            _$baoCaoVanBanDuAnInformationForm.validate();
        };

		          $('#OpenVanBanDuAnLookupTableButton').click(function () {

            var baoCaoVanBanDuAn = _$baoCaoVanBanDuAnInformationForm.serializeFormToObject();

            _BaoCaoVanBanDuAnvanBanDuAnLookupTableModal.open({ id: baoCaoVanBanDuAn.vanBanDuAnId, displayName: baoCaoVanBanDuAn.vanBanDuAnName }, function (data) {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnName]').val(data.displayName); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearVanBanDuAnNameButton').click(function () {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnName]').val(''); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=vanBanDuAnId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var baoCaoVanBanDuAn = _$baoCaoVanBanDuAnInformationForm.serializeFormToObject();

            _BaoCaoVanBanDuAnuserLookupTableModal.open({ id: baoCaoVanBanDuAn.userId, displayName: baoCaoVanBanDuAn.userName }, function (data) {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userName]').val(data.displayName); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userName]').val(''); 
                _$baoCaoVanBanDuAnInformationForm.find('input[name=userId]').val(''); 
        });
		


        this.save = function () {
            if (!_$baoCaoVanBanDuAnInformationForm.valid()) {
                return;
            }
            if ($('#BaoCaoVanBanDuAn_VanBanDuAnId').prop('required') && $('#BaoCaoVanBanDuAn_VanBanDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('VanBanDuAn')));
                return;
            }
            if ($('#BaoCaoVanBanDuAn_UserId').prop('required') && $('#BaoCaoVanBanDuAn_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var baoCaoVanBanDuAn = _$baoCaoVanBanDuAnInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _baoCaoVanBanDuAnsService.createOrEdit(
				baoCaoVanBanDuAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBaoCaoVanBanDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);