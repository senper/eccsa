import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { Grid, Snackbar } from "@mui/material";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";
import {
  getEmailMobileVerification,
  undoEmailMobileChange,
} from "../../api/oauth";
import { getLocalByKey } from "../api/menu";

export default function UndoChange({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };

  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [errorMsg, setErrorMsg] = React.useState(false);
  const [error, setError] = React.useState(false);

  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };

  function handleSubmit(event) {
    undoEmailMobileChange({
      UserName: userName,
      Password: password,
    }).then((sendData) => {
      if (sendData.data.success) {
        setErrorMsg("Email/Mobile # change undo successfully.");
        location.push("/verifyAccount/emailPhoneUndo");
        showNotification("tr", false);
      } else if (sendData.data.errorMsg) {
        setErrorMsg(sendData.data.errorMsg);
        showNotification("tr", false);
      } else {
        setErrorMsg("Failed to send undo email/mobile # change!");
        showNotification("tr");
      }
    });
    event.preventDefault();
  }
  return (
    <>
      <Grid container style={{ paddingTop: "15px" }}>
        <Grid item xs={1} md={3}></Grid>
        <Grid item xs={10} md={6}>
          <div className="row">
            <div className="bg">{getLocal("Undo email/mobile # change")} </div>
          </div>
          <form onSubmit={handleSubmit}>
            <Form.Group size="lg" controlId="userName">
              <Form.Label>{getLocal("User Name")}</Form.Label>
              <Form.Control
                autoFocus
                type="userName"
                value={userName}
                required
                onChange={(e) => setUserName(e.target.value)}
              />
            </Form.Group>
            <Form.Group size="lg" controlId="password">
              <Form.Label>{getLocal("Password")}</Form.Label>
              <Form.Control
                type="password"
                value={password}
                required
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>
            <button type="submit" className="btn btn-outline-success">
              {getLocal("Undo")}
            </button>
          </form>
        </Grid>
        <Grid item xs={1} md={3}></Grid>
      </Grid>
      <br />
      <br />

      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={
          errorMsg ? errorMsg : getLocal("Failed to Undo Email/Mobile # change")
        }
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
