import React, { useState } from "react";
import { Button, Col, Form, InputGroup, Row } from "react-bootstrap";
import { faCheckCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";
import { resetPassword } from "../../api/oauth";

export default function ResetPassword({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [emailCode, setEmailCode] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errorMsg, setErrorMsg] = React.useState("");
  const [error, setError] = React.useState(false);
  let codeValue = 0;
  if (location.query) {
    codeValue = location.query.code;
  }
  React.useEffect(() => {
    if (codeValue && codeValue.trim()) {
      setEmailCode(codeValue.trim());
    }
  }, [codeValue]);

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
    if (confirmPassword !== newPassword) {
      setErrorMsg("Password & Confirm Password are not similar!");
      showNotification("tr", false);
      return;
    }
    resetPassword({
      ResetCode: emailCode,
      NewPassword: newPassword,
    })
      .then((data) => {
        if (data.data.success == false) {
          setErrorMsg(data.data.errorMsg);
          showNotification("tr", false);
        } else if (data.data.success == true) {
          location.push("/resetPassword/passwordReseted");
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
              {getLocal("Reset Password")}{" "}
            </div>
          </div>
          <form onSubmit={handleSubmit}>
            <Row className="g-2">
              <Col md>
                <Form.Label> New Password</Form.Label>

                <Form.Control
                  type="password"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                />
              </Col>
              <Col md>
                <Form.Label> Confirm Password</Form.Label>

                <Form.Control
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
              <Button
                  type="submit"
                  variant="outline-success"
                  style={{ margin: "0" }}
                  id="button-addon2"
                >
                  Reset
                </Button>
              </Col>
            </Row>
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
