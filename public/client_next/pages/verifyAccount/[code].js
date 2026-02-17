import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { faCheckCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";
import { verifyEmailMobile } from "../../api/oauth";

export default function VerifyCertificate({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [emailCode, setEmailCode] = useState("");
  const [mobileCode, setMobileCode] = useState(" ");
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
    verifyEmailMobile({
      emailCode: emailCode,
      mobileNoCode: mobileCode,
    })
      .then((data) => {
        if (data.data.success == false) {
          setErrorMsg(data.data.message);
          showNotification("tr", false);
        } else if (data.data.success == true) {
          location.push("/verifyAccount/emailPhoneConfirmed");
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
              {/* {getLocal("Verify Email/Mobile")}{" "} */}
              {getLocal("Verify Email")}{" "}
            </div>
          </div>
          <form onSubmit={handleSubmit}>
            <Form.Group size="lg" controlId="userName" style={{display: "grid", justifyContent: "center", }}>
              <InputGroup className="mb-3">
                <InputGroup.Text>
                  {/* {getLocal("Mobile Sent Code")} */}
                  {getLocal("Click")} {" "}
                </InputGroup.Text>
                <Form.Control
                  autoFocus
                  type="hidden"
                  value={mobileCode}
                  onChange={(e) => setMobileCode(e.target.value)}
                />
                <Button
                  type="submit"
                  variant="outline-success"
                  style={{ margin: "0" }}
                  id="button-addon2"
                >
                  <FontAwesomeIcon icon={faCheckCircle} />
                </Button>
                <InputGroup.Text>
                {" "}{getLocal("to Verify")}
                </InputGroup.Text>
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
