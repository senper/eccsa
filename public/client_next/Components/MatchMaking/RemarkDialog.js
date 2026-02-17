import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import * as React from "react";
import { Form } from "react-bootstrap";

export default function RemarkDialog(props) {
  const { open, setOpen, title, remark, setRemark, SaveRemark, getLocal } = props;

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div>
      <Dialog
        fullWidth
        maxWidth="sm"
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title" className="alert-info">
          {getLocal("Remark")} for {title}
        </DialogTitle>
        <DialogContent>
          <Form.Label> {getLocal("Remark")}</Form.Label>
          <Form.Control
            as="textarea"
            rows="2"
            value={remark}
            onChange={(evt) => setRemark(evt.target.value)}
          />
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
            onClick={() => SaveRemark()}
            style={{ float: "none", margin: "0" }}
          >
            {getLocal("Save")}
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
