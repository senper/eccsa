import * as React from "react";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import { Form } from "react-bootstrap";

export default function RemarkDialog(props) {
  const { open, setOpen, title, remark, setRemark, SaveRemark } = props;

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
          {"Remark"} for {title}
        </DialogTitle>
        <DialogContent>
          <Form.Label> Remark</Form.Label>
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
            class="btn btn-sm btn-outline-info"
            onClick={handleClose}
            style={{ float: "none", margin: "0 10px 0 0" }}
          >
            Cancel
          </button>{" "}
          {"  "}
          <button
            type="button"
            class="btn btn-sm btn-warning"
            onClick={() => SaveRemark()}
            style={{ float: "none", margin: "0" }}
          >
            Save
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
