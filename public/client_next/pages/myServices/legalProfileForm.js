import React from "react";
import { useForm } from "react-hook-form";
import {
  getLegalProfile,
  LegalDocumentTypes,
  saveLegalProfile,
} from "../../api/legalProfile";
import { Row, Col, Form } from "react-bootstrap";
import { useRouter } from "next/router";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { useSelector } from "react-redux";
import { AddAlert } from "@mui/icons-material";
import Custom404 from "../404";
import Login from "../login";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function LegalProfileForm({ translations }) {
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
  let idValue = null;
  if (location.query) {
    const { id } = location.query;
    idValue = id;
  }
  const [legalProfile, setLegalProfile] = React.useState([]);
  const [readOnly, setReadOnly] = React.useState("");
  const [disabled, setDisabled] = React.useState("");
  React.useEffect(() => {
    if (user && idValue) {
      getLegalProfile(idValue).then((data) => {
        setLegalProfile(data.data);
        const values = getValues();
        for (const [key] of Object.entries(values)) {
          setValue(key, data.data[key]);
        }
        const canEdit = !idValue || !data.data || !data.data.isLocked;
        setReadOnly(canEdit ? "" : "readOnly");
        setDisabled(canEdit ? "" : "disabled");
      });
    }
  }, [user, idValue]);
  function onSubmit(inputData) {
    var formData = new FormData();
    formData.append("id", idValue ? idValue : emptyGuid);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    saveLegalProfile(formData)
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
        showNotification("tr");
      });
  }
  function Redirect() {
    location.push("/myServices?navTo=legalProfile");
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
  const getAttachmentLink = () => {
    if (!legalProfile || !legalProfile.fileExtension) {
      return null;
    }
    const fileName = legalProfile.id
      .substr(legalProfile.id.length - 15)
      .replace("-", "");
    const userIdPath = legalProfile.userId
      .substr(legalProfile.id.length - 15)
      .replace("-", "");
    const relativePath = `Users/LegalProfiles/${userIdPath}/${fileName}${legalProfile.fileExtension}`;
    return (
      <>
        {" "}
        &nbsp;&nbsp;&nbsp;&nbsp;
        <a
          href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
          target="_blank"
          rel="noreferrer"
        >
          {getLocal("Saved Document")}
        </a>
      </>
    );
  };

  if (!user) {
    //return <Custom404 statusCode={404} />;
    return <Login translations={translations} />;
  }
  return (
    <div>
      <Grid container spacing={2}>
        <Grid item xs={3} md={3}></Grid>
        <Grid item xs={6} md={6}>
          <div className="row">
            <div className="bg">
              {idValue ? getLocal("Update") : getLocal("Add")}{" "}
              {getLocal("Legal Profile")}
            </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Type")}</Form.Label>
                <Form.Select
                  {...{ disabled }}
                  {...register("legalDocumentTypeValue", { required: true })}
                  style={{
                    borderColor: formState.errors["legalDocumentType"]
                      ? "red"
                      : "",
                  }}
                  onChange={(event) => {
                    if (!idValue) {
                      const selected = LegalDocumentTypes.find(
                        (lt) => lt.value == event.target.value
                      );
                      if (selected) {
                        setValue("title", selected.name);
                      }
                    }
                  }}
                >
                  <option></option>
                  {LegalDocumentTypes.map((value, index) => (
                    <option key={index} value={value.value}>
                      {value.name}
                    </option>
                  ))}
                </Form.Select>
              </Col>
              <Col md>
                <Form.Label> {getLocal("Title")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("title", { required: true, maxLength: 50 })}
                  style={{
                    borderColor: formState.errors["title"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Document Number")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="documentNumber"
                  {...register("documentNumber", {
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["documentNumber"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("Expiry Date")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="date"
                  {...register("expiryDateStr")}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Group controlId="formFile" className="mb-3">
                  <Form.Label>
                    {getLocal("Attachment")}
                    {getAttachmentLink()}
                  </Form.Label>
                  <Form.Control
                    {...{ disabled }}
                    type="file"
                    {...register("attachment", { required: !legalProfile })}
                  />
                </Form.Group>
              </Col>
              <Col md>
                <Form.Label> {getLocal("Status")}</Form.Label>
                <Form.Select
                  {...register("statusValue", { required: true })}
                  style={{
                    borderColor: formState.errors["status"] ? "red" : "",
                  }}
                >
                  <option value="1">{getLocal("Active")}</option>
                  <option value="0">{getLocal("Inactive")}</option>
                </Form.Select>
              </Col>
            </Row>
            <button type="submit" className="btn btn-success">
              {getLocal("Save")}
            </button>
            <button
              type="button"
              className="btn btn-warning"
              onClick={Redirect}
            >
              {getLocal("Cancel")}
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={3} md={3}></Grid>
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
