import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { faCheckCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";
import { getPasswordResetLink } from "../../api/oauth";

export default function PasswordReset({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [mobileCode, setMobileCode] = useState("");
  const [email, setEmail] = useState("");
  const [errorMsg, setErrorMsg] = React.useState("");
  const [error, setError] = React.useState(false);

  const showNotification = (place, useTimeOut = true) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          if (useTimeOut)
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
    const link = `${process.env.NEXT_PUBLIC_BASE_URL}/resetPassword`;
    getPasswordResetLink({email: email, ConfirmationUrl: link})
      .then((sendData) => {
        if (sendData.data.success) {
          setErrorMsg(
            "Password Reset Link send. check your email."
          );
          showNotification("tr", false);
          setEmail("");
        } else if (sendData.data.errorMsg) {
          setErrorMsg(sendData.data.errorMsg);
          showNotification("tr", false);
        } else {
          setErrorMsg("Failed to send Password Reset Link!");
          showNotification("tr");
        }
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          // setErrorMsg(error.response.data.toString());
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
            <div className="bg" style={{ fontSize: "20px", marginTop: "0" }}>
              {getLocal("Send Password Reset Link")}{" "}
            </div>
          </div>
          <form onSubmit={handleSubmit}>
            <Form.Group size="lg" controlId="userName">
              <InputGroup className="mb-3">
                <InputGroup.Text>
                  {getLocal("Email")}
                </InputGroup.Text>
                <Form.Control
                  autoFocus
                  type="text"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <Button
                  type="submit"
                  variant="outline-success"
                  style={{ margin: "0" }}
                  id="button-addon2"
                >
                  Send
                </Button>
              </InputGroup>
            </Form.Group>
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
          errorMsg ? errorMsg : getLocal("Failed to verify Email/Mobile No")
        }
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
