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

export default function NeedEmailPhoneConfirmation({ translations }) {
  const location = useRouter();
  const { locale } = location;
  let isOld = false;
  if (location.query) {
    const { c } = location.query;
    isOld = c && c == 1;
  }

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [email, setEmail] = useState("");
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

  function handleSubmit(event, undo) {
    if (undo) {
      location.push("/verifyAccount/undoChange");
    }
    if (!email || !email.length || !email.trim || email.trim().length <= 0) {
      setErrorMsg("Enter Email");
      showNotification("tr");
    } else {
      const link = `${process.env.NEXT_PUBLIC_BASE_URL}/verifyAccount`;
      getEmailMobileVerification({
        email: email,
        ConfirmationUrl: link,
      }).then((sendData) => {
        if (sendData.data.success) {
          setErrorMsg("Verification codes sent. check your email & mobile No.");
          showNotification("tr", false);
        } else if (sendData.data.errorMsg) {
          setErrorMsg(sendData.data.errorMsg);
          showNotification("tr", false);
        } else {
          setErrorMsg("Failed to send verification code!");
          showNotification("tr");
        }
      });
    }
    event.preventDefault();
  }
  return (
    <>
      <Grid container style={{ paddingTop: "15px" }}>
        <Grid item xs={1} md={3}></Grid>
        <Grid item xs={10} md={6}>
          <div className="row">
            <div className="bg" style={{ fontSize: "20px", marginTop: "0" }}>
              {getLocal("Verify Email/Mobile")}{" "}
            </div>
          </div>
          <h4>
            <strong>
              {getLocal(
                "Check your email and click confirmation link to confirmation your email and mobile no!"
              )}
            </strong>
          </h4>
          <strong> OR </strong>
          <br />
          Enter Email{" "}
          <Form.Control
            type="text"
            value={email}
            placeholder="Email"
            style={{ margin: "10px" }}
            onChange={(e) => {
              setEmail(e.target.value);
            }}
          />
          Click{" "}
          <Button
            size="sm"
            type="submit"
            variant="outline-success"
            style={{ margin: "0", float: "none" }}
            id="button-addon2"
            onClick={(evt) => handleSubmit(evt, false)}
          >
            Resend
          </Button>{" "}
          to get new Confirmation codes.
          {isOld ? (
            <>
              <br />
              <strong> OR </strong> <br />
              Click{" "}
              <Button
                size="sm"
                type="submit"
                variant="outline-success"
                style={{ margin: "0", float: "none" }}
                id="button-addon2"
                onClick={(evt) => handleSubmit(evt, true)}
              >
                Undo
              </Button>
              {"  "}
              to undo email/mobile # change.
            </>
          ) : null}
          <br />
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
          errorMsg ? errorMsg : getLocal("Failed to verify Email/Mobile No")
        }
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
