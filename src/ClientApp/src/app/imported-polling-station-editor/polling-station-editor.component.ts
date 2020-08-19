import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatTableDataSource } from '@angular/material';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ImportedPollingStation, AssignedAddress } from '../services/data.service';
import { Store } from '@ngrx/store';
import { ApplicationState } from '../state/reducers';
import { CreateImportedPollingStationAction, UpdateImportedPollingStationAction } from '../state/actions';
import { get, isEqual } from 'lodash';

@Component({
  selector: 'app-polling-station-editor',
  templateUrl: './polling-station-editor.component.html',
  styleUrls: ['./polling-station-editor.component.scss']
})
export class PollingStationEditorComponent implements OnInit {
  form: FormGroup;
  assignedAddresses: MatTableDataSource<AssignedAddress>;
  assignedAddressesColumns = ['streetCode', 'street', 'houseNumbers', 'remarks', 'delete'];

  constructor(
    public dialogRef: MatDialogRef<PollingStationEditorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { jobId: string, pollingStation: ImportedPollingStation },
    public store: Store<ApplicationState>
  ) {
    this.dialogRef.disableClose = true;
    this.form = new FormGroup({
      id: new FormControl(null),
      jobId: new FormControl(null),
      latitude: new FormControl(null),
      longitude: new FormControl(null),
      county: new FormControl('', Validators.required),
      locality: new FormControl('', Validators.required),
      pollingStationNumber: new FormControl('', Validators.required),
      institution: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
      resolvedAddressStatus: new FormControl('notProcessed'),
      failMessage: new FormControl(null)
    });

    if (data && data.pollingStation) {
      this.form.patchValue(data.pollingStation);
    }

    this.assignedAddresses = new MatTableDataSource(get(data.pollingStation, 'assignedAddresses', []));
  }

  ngOnInit() {

  }

  onSubmit() {
    if (this.form.value.id) {
      this.store.dispatch(new UpdateImportedPollingStationAction(this.data.jobId, this.form.value.id, this.form.value, this.assignedAddresses.data));
    } else {
      this.store.dispatch(new CreateImportedPollingStationAction(this.data.jobId, this.form.value, this.assignedAddresses.data));
    }

    this.dialogRef.close();
  }

  newAddress(): void {
    this.assignedAddresses.data = [
      {
        houseNumbers: '',
        remarks: '',
        street: '',
        streetCode: ''
      },
      ...this.assignedAddresses.data];
  }

  deleteAddress(index: any): void {
    const addresses = this.assignedAddresses.data;
    addresses.splice(index, 1);
    this.assignedAddresses.data = [...addresses];
  }

  onClose(): void {
    this.dialogRef.close();
  }
}