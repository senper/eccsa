import * as React from "react";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

export default function ConfirmDialog(props) {
  const { open, setOpen, resultAction, text, btnValue, param, getLocal } = props;

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
          {getLocal("Confirm")}?
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            {text}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <button
            type="button"
            className="btn btn-sm btn-outline-info"
            onClick={handleClose}
            style={{ float: "none", margin: "0 10px 0 0" }}
          >
            {getLocal("Cancel")}
          </button>{" "}
          {"  "}
          <button
            type="button"
            className="btn btn-sm btn-warning"
            onClick={() => resultAction(param)}
            style={{ float: "none", margin: "0" }}
          >
            {btnValue}
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
