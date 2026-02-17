import React from "react";
import { useForm } from "react-hook-form";
import {
  ContactTypes,
  getUserContact,
  saveUserContact,
} from "../api/userContact";
import { Row, Col, Form } from "react-bootstrap";
import { useRouter } from "next/router";
import { useSelector } from "react-redux";
import { Grid, Snackbar } from "@mui/material";
import { AddAlert } from "@mui/icons-material";
import { getLocalByKey } from "../api/menu";
import Custom404 from "../404";
import Login from "../login";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function UserContactForm({ translations }) {
  window.scrollTo({
    top: 0,
  });
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );

  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState, //: { errors },
  } = useForm();
  let serviceTypeIdValue = 0;
  let idValue = null;
  if (location.query) {
    const { id } = location.query;
    idValue = id;
  }
  const [userContact, setUserContact] = React.useState([]);
  const [readOnly, setReadOnly] = React.useState("");
  const [disabled, setDisabled] = React.useState("");
  React.useEffect(() => {
    if (user && idValue) {
      getUserContact(idValue).then((data) => {
        setUserContact(data.data);
        const values = getValues();
        for (const [key, value] of Object.entries(values)) {
          setValue(key, data.data[key]);
        }
        const canEdit = !idValue || !data.data || !data.data.isLocked;
        setReadOnly(canEdit ? "" : "readOnly");
        setDisabled(canEdit ? "" : "disabled");
      });
    }
  }, [user, idValue]);
  function onSubmit(inputData) {
    inputData.id = idValue ? idValue : emptyGuid;
    saveUserContact(inputData)
      .then((data) => {
        if (data.data.saved) {
          Redirect();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        // console.log(error.response);
        showNotification("tr");
      });
  }
  function Redirect() {
    location.push("/myServices?navTo=contacts");
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };

  if (!user) {
    //return <Custom404 statusCode={404} />;
    return <Login translations={translations} />;
  }
  return (
    <div>
      <Grid container spacing={2}>
        <Grid item xs={3} md={3} ></Grid>
        <Grid item xs={6} md={6} >
          <div className="row">
            <div className="bg">
              {idValue ? getLocal("Update") : getLocal("Add")}{" "}
              {getLocal("Contact")}
            </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
            <input type="hidden" {...register("id")} />
            {/* {setValue("id", idValue ? idValue : emptyGuid)} */}
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Priority")}</Form.Label>
                <Form.Control
                  type="number"
                  {...register("priority", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["priority"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("Type")}</Form.Label>
                <Form.Select
                  {...register("contactType", { required: true })}
                  style={{
                    borderColor: formState.errors["contactType"] ? "red" : "",
                  }}
                >
                  <option></option>
                  {ContactTypes.map((value, index) => (
                    <option key={index} value={value.name}>{value.name}</option>
                  ))}
                </Form.Select>
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Address")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("address", { required: true, maxLength: 255 })}
                  style={{
                    borderColor: formState.errors["address"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <button type="submit" className="btn btn-success">
              {getLocal("Save")}
            </button>
            <button type="button" className="btn btn-warning" onClick={Redirect}>
              {getLocal("Cancel")}
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={3} md={3} ></Grid>
        <Grid container spacing={2}>
          <br />
          <br />
        </Grid>

        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : getLocal("Failed to Save")}
          open={error}
          onClose={() => setError(false)}
        />
      </Grid>
    </div>
  );
}

// export default App;
