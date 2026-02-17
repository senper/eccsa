import * as React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

export default function ConfirmDialog(props) {
  const {open, setOpen, resultAction, text, btnValue, param } = props;

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title" className="alert-info">
          {"Confirm?"}
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            {text}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <button type="button" class="btn btn-sm btn-outline-info" onClick={handleClose} style={{float: "none", margin: "0 10px 0 0"}} >
                Cancel
          </button>     {"  "}     
          <button type="button" class="btn btn-sm btn-warning" onClick={() => resultAction(param)} style={{float: "none", margin: "0"}} >
                {btnValue}
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}