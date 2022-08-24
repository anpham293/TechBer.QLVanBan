(function () {
    $(function () {

        var _$loaiDuAnsTable = $('#LoaiDuAnsTable');
        var _loaiDuAnsService = abp.services.app.loaiDuAns;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.LoaiDuAn';
        var _$quyTrinhDuAnsTable = $('#QuyTrinhDuAnsTable');
        var find_group = $('#find');
        var _quyTrinhDuAnsService = abp.services.app.quyTrinhDuAns;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var loai_id=0;
        var _permissions = {
            create: abp.auth.hasPermission('Pages.LoaiDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.LoaiDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.LoaiDuAns.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LoaiDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLoaiDuAnModal'
        });
        var _createOrEditQuyTrinhModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditQuyTrinhDuAnModal'
        });

        var _viewLoaiDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiDuAns/ViewloaiDuAnModal',
            modalClass: 'ViewLoaiDuAnModal'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$loaiDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _loaiDuAnsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#LoaiDuAnsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        organizationUnitDisplayNameFilter: $('#OrganizationUnitDisplayNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewLoaiDuAnModal.open({id: data.record.loaiDuAn.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.loaiDuAn.id});
                                }
                            },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.loaiDuAn.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteLoaiDuAn(data.record.loaiDuAn);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "loaiDuAn.name",
                    name: "name",
                    render: function (displayName, type, row, meta) {
                        return "<a style='color:#0a6aa1;cursor: pointer' class='filter-quytrinh btn-reload-vanban' data-target='"+row.loaiDuAn.id+"'>"+row.loaiDuAn.name+"</a>";
                    }
                },
                {
                    targets: 2,
                    data: "organizationUnitDisplayName",
                    name: "organizationUnitFk.displayName"
                }
            ]
        });

        function getLoaiDuAns() {
            dataTable.ajax.reload();
        }
        
        class Cay {
            XOA_STATE = {
                XOA_THANH_CONG: 1,
                CON_NODE_CON: 2,
                CO_LOI: 3
            }

            DI_CHUYEN_STATE = {
                THANH_CONG: 1,
                THAT_BAI: 2
            }

            constructor(id, rootBtn) {
                this.eleId = id;
                this.$tree = $('#' + id);
                this.loaiDuAn = 0;
                this.show = function () {
                    this.$tree.show();
                };
                this.hide = function () {
                    this.$tree.hide();
                };
                this.unitCount = 0;
                this.$rootBtn = $('#' + rootBtn);
            }

            setUnitCount(unitCount) {
                this.unitCount = unitCount;
                if (unitCount) {
                    this.show();
                } else {
                    this.hide();
                }
            }

            selectedOu = {
                id: null,
                displayName: null,
                code: null,

                set(ouInTree) {
                    if (!ouInTree) {
                        this.selectedOu.id = null;
                        this.selectedOu.displayName = null;
                        this.selectedOu.code = null;
                    } else {
                        this.selectedOu.id = ouInTree.id;
                        this.selectedOu.displayName = ouInTree.original.displayName;
                        this.selectedOu.code = ouInTree.original.code;
                    }

                }
            }

            refreshUnitCount() {
                this.setUnitCount(this.$tree.jstree('get_json').length);
            }

            addUnit(parentId) {
                let self = this

                var instance = $.jstree.reference(self.$tree);

                _createOrEditQuyTrinhModal.open({
                    parentId: parentId,
                    loaiDuAn: self.loaiDuAn
                }, function (newOu) {
                    instance.create_node(
                        parentId ? instance.get_node(parentId) : '#',
                        {
                            id: newOu.quyTrinhDuAn.id,
                            parent: newOu.quyTrinhDuAn.parentId ? newOu.quyTrinhDuAn.parentId : '#',
                            code: newOu.quyTrinhDuAn.maQuyTrinh,
                            displayName: newOu.quyTrinhDuAn.displayName,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(newOu.quyTrinhDuAn),
                            state: {
                                opened: true
                            }
                        });
                    self.refreshUnitCount();
                    self.reload();
                });
            }

            generateTextOnTree(ou) {
                var itemClass = ' ou-text-has-members';
                
                return '<span title="' + ou.name + '" class="ou-text text-dark' + itemClass + '" data-ou-id="' + ou.id + '"><b>' + ou.maQuyTrinh + '</b> ' +
                    app.htmlUtils.htmlEncodeText(ou.name) +
                    ' <i class="fa fa-caret-down text-muted"></i> ' +
                    ' <span style="font-size: .82em; opacity: .5;">' +
                    '</span></span>';
            }

            incrementMemberCount(ouId, incrementAmount) {
                var treeNode = this.$tree.jstree('get_node', ouId);
                treeNode.original.memberCount = treeNode.original.memberCount + incrementAmount;
                this.$tree.jstree('rename_node', treeNode, this.generateTextOnTree(treeNode.original));
            }

            incrementRoleCount(ouId, incrementAmount) {
                var treeNode = this.$tree.jstree('get_node', ouId);
                treeNode.original.roleCount = treeNode.original.roleCount + incrementAmount;
                this.$tree.jstree('rename_node', treeNode, this.generateTextOnTree(treeNode.original));
            }

            getTreeDataFromServer(loai_id,callback) {
                let self = this;
                _quyTrinhDuAnsService.getDataForTree(loai_id).done(function (result) {
                    var treeData = _.map(result, function (item) {
                       
                        return {
                            id: item.quyTrinhDuAn.id,
                            parent: item.quyTrinhDuAn.parentId ? item.quyTrinhDuAn.parentId : '#',
                            code: '0001',
                            displayName: item.quyTrinhDuAn.stt + ' ' + item.quyTrinhDuAn.name,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(item.quyTrinhDuAn),
                            sapXep: item.quyTrinhDuAn.stt ?? 0,
                            state: {
                                // opened: true
                            }
                        };
                    });

                    callback(treeData);
                });
            }

            init(loai_id) {
                let self = this;
                abp.ui.setBusy('#' + self.eleId)

                this.loaiDuAn = loai_id;


                this.getTreeDataFromServer(loai_id,function (treeData) {

                    self.setUnitCount(treeData.length)
                    self.$tree.on('changed.jstree', function (e, data) {
                    })
                        .on('move_node.jstree', function (e, data) {

                            console.log(data);

                            var parentNodeName = (!data.parent || data.parent === '#')
                                ? app.localize('Root')
                                : self.$tree.jstree('get_node', data.parent).original.displayName;

                            abp.message.confirm(
                                app.localize('XacNhanDiChuyenTieuChi', data.node.original.displayName, parentNodeName),
                                app.localize('AreYouSure'),
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        _quyTrinhDuAnsService.moveTieuChi({
                                            id: data.node.id,
                                            position: data.position,
                                            newParentId: data.parent === '#' ? null : data.parent
                                        }).done(function (ketQua) {
                                            if (ketQua == self.DI_CHUYEN_STATE.THANH_CONG) {
                                                abp.notify.success(app.localize('SuccessfullyMoved'));
                                                self.reload();
                                            } else {
                                                self.$tree.jstree('refresh'); //rollback
                                                setTimeout(function () {
                                                    abp.message.error("Di chuyển thất bại");
                                                }, 500);
                                            }
                                        }).fail(function (err) {
                                            self.$tree.jstree('refresh'); //rollback
                                            setTimeout(function () {
                                                abp.message.error(err.message);
                                            }, 500);
                                        });
                                    } else {
                                        self.$tree.jstree('refresh'); //rollback
                                    }
                                }
                            );
                        })
                        .jstree({
                            'core': {
                                data: treeData,
                                multiple: false,
                                check_callback: function (operation, node, node_parent, node_position, more) {
                                    abp.ui.clearBusy();
                                    return true;
                                }
                            },
                            types: {
                                "default": {
                                    "icon": "fa fa-folder kt--font-warning"
                                },
                                "file": {
                                    "icon": "fa fa-file  kt--font-warning"
                                }
                            },
                            contextmenu: {
                                items: function (node) {
                                    var items = {
                                        editUnit: {
                                            label: app.localize('Edit'),
                                            icon: 'la la-pencil',
                                            _disabled: !_permissions.edit,
                                            action: function (data) {
                                                var instance = $.jstree.reference(data.reference);
                                                console.log(node);
                                                _createOrEditQuyTrinhModal.open({id: parseInt(node.id), parentId: parseInt(node.parent),
                                                    loaiDuAn: self.loaiDuAn}, function (updatedOu) {
                                                    self.reload();
                                                    //console.log(node)
                                                    //node.original.sapXep = updatedOu.displayName;
                                                    //instance.rename_node(node, self.generateTextOnTree(updatedOu.quyTrinhDuAn));
                                                });
                                            }
                                        },

                                        addSubUnit: {
                                            label: 'Thêm quy trình con',
                                            icon: 'la la-plus',
                                            _disabled: !_permissions.create,
                                            action: function () {
                                                self.addUnit(node.id);
                                            }
                                        },

                                        'delete': {
                                            label: app.localize("Delete"),
                                            icon: 'la la-remove',
                                            _disabled: !_permissions.delete,
                                            action: function (data) {
                                                var instance = $.jstree.reference(data.reference);

                                                abp.message.confirm(
                                                    app.localize('XoaTieuChi', node.original.displayName),
                                                    app.localize('AreYouSure'),
                                                    function (isConfirmed) {
                                                        if (isConfirmed) {
                                                            _quyTrinhDuAnsAppService.xoaTieuChi(node.id).done(function (result) {
                                                                if (result == self.XOA_STATE.XOA_THANH_CONG) {
                                                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                                                    instance.delete_node(node);
                                                                    self.refreshUnitCount();
                                                                } else if (result == self.XOA_STATE.CON_NODE_CON) {
                                                                    abp.message.warn(
                                                                        'Không thể xóa khi vẫn còn tiêu chí con',
                                                                        'Lưu ý'
                                                                    )
                                                                } else {
                                                                    abp.message.warn(
                                                                        'Có lỗi xảy ra',
                                                                        'Lưu ý'
                                                                    )
                                                                }

                                                            }).fail(function (err) {
                                                                setTimeout(function () {
                                                                    abp.message.error(err.message);
                                                                }, 500);
                                                            });
                                                        }
                                                    }
                                                );
                                            }
                                        }
                                    };

                                    if (entityHistoryIsEnabled()) {
                                        items.history = {
                                            label: app.localize('History'),
                                            icon: 'la la-history',
                                            _disabled: !_permissions.cayTieuChi,
                                            action: function () {
                                                _entityTypeHistoryModal.open({
                                                    entityTypeFullName: _entityTypeFullName,
                                                    entityId: node.original.id,
                                                    entityTypeDescription: node.original.displayName
                                                });
                                            }
                                        };
                                    }

                                    return items;
                                }
                            },
                            sort: function (node1, node2) {
                                if (parseInt(this.get_node(node2).original.sapXep) < parseInt(this.get_node(node1).original.sapXep)) {
                                    return 1;
                                }
                                return -1;
                            },
                            plugins: [
                                'types',
                                'contextmenu',
                                'wholerow',
                                'sort',
                                'state',
                                'dnd'
                            ]
                        });
                    find_group.removeAttr("hidden");
                    self.$rootBtn.click(function (e) {
                        e.preventDefault();
                        self.addUnit();
                    });

                   
                    self.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                        e.preventDefault();

                        var ouId = $(this).closest('.ou-text').attr('data-ou-id');
                        setTimeout(function () {
                            self.$tree.jstree('show_contextmenu', ouId);
                        }, 100);
                    });
                    self.$tree.on('click', '.jstree-icon.jstree-ocl', function (e) {
                        e.preventDefault();
                        self.$tree.jstree('save_state');
                    });
                    $("#Collapse").on("click",function (){
                        var bt = $(this);
                        self.$tree.jstree('close_all');
                        $("#Expand").removeAttr("hidden");
                        bt.attr("hidden","hidden");
                        self.$tree.jstree('save_state');
                    });
                    $("#Expand").on("click",function (){
                        var bt = $(this);
                        self.$tree.jstree('open_all');
                        $("#Collapse").removeAttr("hidden");
                        bt.attr("hidden","hidden");
                        self.$tree.jstree('save_state');
                    });
                    $("#reload-tree").on("click",function (){
                        self.$tree.jstree('save_state');
                        self.reload();
                    });
                });
            }

            reload() {
                let self = this;
                self.getTreeDataFromServer(loai_id,function (treeData) {
                    find_group.removeAttr("hidden");
                    self.setUnitCount(treeData.length);
                    self.$tree.jstree(true).settings.core.data = treeData;
                    self.$tree.jstree('refresh');
                });
            }
        }

       
        $(document).on("click",".filter-quytrinh",function () {
            var self=$(this);
            var trees=$("#tree");
            var d = trees.parent();
            trees.remove();
            loai_id=self.attr("data-target");
            d.append("<div id='tree'></div>");
            var CayPhuLucIA = new Cay('tree', 'AddRootUnitButtonIA');
            CayPhuLucIA.init(loai_id);
        });
        function deleteLoaiDuAn(loaiDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _loaiDuAnsService.delete({
                            id: loaiDuAn.id
                        }).done(function () {
                            getLoaiDuAns(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }
        $(document).on("click", ".btn-reload-vanban", function () {
            var self = $(this);
            $(".text-danger").removeClass("text-danger").addClass("text-info");
            $('#QuyTrinhDuAnNameFilterId').val(self.attr("data-target"));
            self.removeClass("text-info").addClass("text-danger");
            getLoaiDuAns();
        });
        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewLoaiDuAnButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _loaiDuAnsService
                .getLoaiDuAnsToExcel({
                    filter: $('#LoaiDuAnsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    organizationUnitDisplayNameFilter: $('#OrganizationUnitDisplayNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLoaiDuAnModalSaved', function () {
            getLoaiDuAns();
        });

        $('#GetLoaiDuAnsButton').click(function (e) {
            e.preventDefault();
            getLoaiDuAns();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLoaiDuAns();
            }
        });
    });
})();