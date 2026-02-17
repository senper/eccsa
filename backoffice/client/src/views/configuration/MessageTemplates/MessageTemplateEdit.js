import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import { serviceTypes } from "variables/lookups.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";

import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import TextareaAutosize from "@material-ui/core/TextareaAutosize";
import MenuItem from "@material-ui/core/MenuItem";
import { Check, Info } from "@material-ui/icons";
import { Link, useLocation } from "react-router-dom";
import { Checkbox, FormControlLabel, FormLabel } from "@material-ui/core";
import CustomInput from "components/vender/CustomInput/CustomInput";
import Button from "components/vender/CustomButtons/Button.js";

import styles from "assets/jss/material-dashboard-pro-react/views/regularFormsStyle";

//import { CKEditor } from "@ckeditor/ckeditor5-react";
//import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

import { CKEditor } from "ckeditor4-react";

import {
  responsibleUnits,
  mediaTypes,
  topics,
  messageTypes,
  placeholders,
} from "variables/lookups.js";
import FileDownloadUpload from "components/common/FileUploadDownload/FileDownloadUpload";

const useStyles = makeStyles(styles);

export default function MessageTemplateEdit() {
  const location = useLocation();
  const [media_type, setMediaType] = React.useState(1);
  const [placeholder, setPlaceholder] = React.useState(0);
  const [message, setMessage] = React.useState("");
  const [ckEditor, setckEditor] = React.useState(null);
  const [attachedFile, setAttachedFile] = React.useState(null);

  const classes = useStyles();
  const handleMediaTypeChange = (event) => {
    console.log(message);
    setMediaType(event.target.value);
    setMessage("");
    if (ckEditor) {
      ckEditor.editor.setData("");
    }
  };
  const addPlaceholderToMessage = () => {
    const phText = placeholders.filter((value) => value.id == placeholder);
    if (phText.length > 0) {
      const text = "{[" + phText[0].name + "]}";
      ckEditor.editor.insertText(text);
    }
  };

  const [responsibleUnit, setResponsibleUnit] = React.useState("");
  React.useEffect(() => {
    if (location.state) {
      setResponsibleUnit(location.state.unit);
    }
  }, [location]);

  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">{"Edit Media Template"}</CardIcon>
            <Link
              style={{ color: "orange" }}
              to="/configuration/message_templates"
            >
              {"<< back to list"}
            </Link>
          </CardHeader>
          <CardBody>
            <GridContainer>
              <GridItem xs={12} sm={4} md={3}>
                <FormControl fullWidth className={classes.selectFormControl}>
                  <InputLabel className={classes.selectLabel}>
                    Responsible Unit
                  </InputLabel>
                  <Select
                    MenuProps={{
                      className: classes.selectMenu,
                    }}
                    classes={{
                      select: classes.select,
                    }}
                    value={responsibleUnit}
                    inputProps={{
                      name: "ResponsibleUnit",
                      id: "responsible-unit",
                      disabled: true,
                    }}
                  >
                    <MenuItem
                      disabled
                      classes={{
                        root: classes.selectMenuItem,
                      }}
                    >
                      Choose Responsible Unit
                    </MenuItem>
                    {responsibleUnits.map((_item, index) => {
                      return (
                        <MenuItem
                          key={_item.id}
                          classes={{
                            root: classes.selectMenuItem,
                            selected: classes.selectMenuItemSelected,
                          }}
                          value={_item.id}
                        >
                          {_item.name}
                        </MenuItem>
                      );
                    })}
                  </Select>
                </FormControl>
              </GridItem>
              <GridItem xs={12} sm={4} md={3}>
                <div className={classes.checkboxAndRadio}>
                  <FormControlLabel
                    control={
                      <Checkbox
                        tabIndex={-1}
                        // onClick={}
                        checkedIcon={<Check className={classes.checkedIcon} />}
                        icon={<Check className={classes.uncheckedIcon} />}
                        classes={{
                          checked: classes.checked,
                          root: classes.checkRoot,
                        }}
                      />
                    }
                    classes={{
                      label: classes.label,
                      root: classes.labelRoot,
                    }}
                    label="Is Recurrent"
                  />
                </div>
              </GridItem>
              <GridItem xs={12} sm={4} md={3}>
                <div className={classes.checkboxAndRadio}>
                  <FormControlLabel
                    control={
                      <Checkbox
                        tabIndex={-1}
                        // onClick={}
                        checkedIcon={<Check className={classes.checkedIcon} />}
                        icon={<Check className={classes.uncheckedIcon} />}
                        classes={{
                          checked: classes.checked,
                          root: classes.checkRoot,
                        }}
                      />
                    }
                    classes={{
                      label: classes.label,
                      root: classes.labelRoot,
                    }}
                    label="Is Subscribable"
                  />
                </div>
              </GridItem>
            </GridContainer>
            <GridContainer>
              <GridItem xs={12} sm={4} md={3}>
                <FormControl fullWidth className={classes.selectFormControl}>
                  <InputLabel className={classes.selectLabel}>Topic</InputLabel>
                  <Select
                    MenuProps={{
                      className: classes.selectMenu,
                    }}
                    classes={{
                      select: classes.select,
                    }}
                    inputProps={{
                      name: "Topic",
                      id: "topic",
                    }}
                  >
                    <MenuItem
                      disabled
                      classes={{
                        root: classes.selectMenuItem,
                      }}
                    >
                      Choose Topic
                    </MenuItem>
                    {topics.map((_item, index) => {
                      return (
                        <MenuItem
                          key={_item.id}
                          classes={{
                            root: classes.selectMenuItem,
                            selected: classes.selectMenuItemSelected,
                          }}
                          value={_item.id}
                        >
                          {_item.name}
                        </MenuItem>
                      );
                    })}
                  </Select>
                </FormControl>
              </GridItem>
              <GridItem xs={12} sm={4} md={3}>
                <FormControl fullWidth className={classes.selectFormControl}>
                  <InputLabel className={classes.selectLabel}>
                    Message Type
                  </InputLabel>
                  <Select
                    MenuProps={{
                      className: classes.selectMenu,
                    }}
                    classes={{
                      select: classes.select,
                    }}
                    inputProps={{
                      name: "MessageType",
                      id: "message-type",
                    }}
                  >
                    <MenuItem
                      disabled
                      classes={{
                        root: classes.selectMenuItem,
                      }}
                    >
                      Choose Message Type
                    </MenuItem>
                    {messageTypes.map((_item, index) => {
                      return (
                        <MenuItem
                          key={_item.id}
                          classes={{
                            root: classes.selectMenuItem,
                            selected: classes.selectMenuItemSelected,
                          }}
                          value={_item.id}
                        >
                          {_item.name}
                        </MenuItem>
                      );
                    })}
                  </Select>
                </FormControl>
              </GridItem>
              <GridItem xs={12} sm={4} md={3}>
                <FormControl fullWidth className={classes.selectFormControl}>
                  <InputLabel className={classes.selectLabel}>
                    Media Type
                  </InputLabel>
                  <Select
                    MenuProps={{
                      className: classes.selectMenu,
                    }}
                    classes={{
                      select: classes.select,
                    }}
                    inputProps={{
                      name: "MediaType",
                      id: "media-type",
                    }}
                    onChange={handleMediaTypeChange}
                  >
                    <MenuItem
                      disabled
                      classes={{
                        root: classes.selectMenuItem,
                      }}
                    >
                      Choose Media Type
                    </MenuItem>
                    {mediaTypes.map((_item, index) => {
                      return (
                        <MenuItem
                          key={_item.id}
                          classes={{
                            root: classes.selectMenuItem,
                            selected: classes.selectMenuItemSelected,
                          }}
                          value={_item.id}
                        >
                          {_item.name}
                        </MenuItem>
                      );
                    })}
                  </Select>
                </FormControl>
              </GridItem>
            </GridContainer>
            <GridContainer>
              <GridItem xs={12} sm={8}>
                {media_type == 2 ? (
                  <CustomInput
                    labelText="Subject"
                    id="subject"
                    formControlProps={{
                      fullWidth: true,
                    }}
                    inputProps={{
                      type: "text",
                    }}
                  />
                ) : null}
              </GridItem>
            </GridContainer>
            <GridContainer>
              <GridItem xs={12} sm={8}>
                <br />
                <InputLabel className={classes.selectLabel}>
                  Message{" "}
                  <strong>
                    {media_type == 1 ? "(maximum of 120 characters)" : ""}
                  </strong>
                </InputLabel>
                {media_type != 1 && media_type != 4 ? (
                  <CKEditor
                    childRef={(ref) => setckEditor(ref)}
                    data={message}
                    onInstanceReady={(editor) => {
                      setckEditor(editor);
                    }}
                    onChange={(event) => setMessage(event.editor.getData())}
                  />
                ) : (
                  <FormControl fullWidth className={classes.selectFormControl}>
                    <textarea
                      rows={3}
                      width="100%"
                      onChange={(event) => setMessage(event.target.value)}
                    ></textarea>
                  </FormControl>
                )}
              </GridItem>
              <GridItem xs={12} sm={4}>
                {media_type != 1 ? (
                  <>
                    <br />
                    <FormControl
                      fullWidth
                      className={classes.selectFormControl}
                    >
                      <InputLabel className={classes.selectLabel}>
                        Placeholders
                      </InputLabel>
                      <Select
                        MenuProps={{
                          className: classes.selectMenu,
                        }}
                        classes={{
                          select: classes.select,
                        }}
                        inputProps={{
                          name: "Placeholder",
                          id: "placeholder",
                        }}
                        value={placeholder}
                        onChange={(event) => setPlaceholder(event.target.value)}
                      >
                        <MenuItem
                          disabled
                          classes={{
                            root: classes.selectMenuItem,
                          }}
                          value={0}
                        >
                          Choose Placeholder
                        </MenuItem>
                        {placeholders.map((_item, index) => {
                          return (
                            <MenuItem
                              key={_item.id}
                              classes={{
                                root: classes.selectMenuItem,
                                selected: classes.selectMenuItemSelected,
                              }}
                              value={_item.id}
                            >
                              {_item.name}
                            </MenuItem>
                          );
                        })}
                      </Select>
                      <Button
                        color="primary"
                        size="sm"
                        onClick={addPlaceholderToMessage}
                      >
                        {"Add"}
                      </Button>
                    </FormControl>
                  </>
                ) : null}
              </GridItem>
            </GridContainer>
            <GridContainer>
              <GridItem xs={12} sm={8}>
                {media_type == 2 || media_type == 4 ? (
                  <FileDownloadUpload
                    setFile={setAttachedFile}
                  ></FileDownloadUpload>
                ) : null}
              </GridItem>
            </GridContainer>
            <GridContainer>
              <GridItem xs={12} sm={6}>
                <Button color="warning" size="sm">
                  <Link
                    style={{ color: "white" }}
                    to="/configuration/message_templates"
                  >
                    {"Save"}
                  </Link>
                </Button>
                <Button color="behance" size="sm">
                  <Link
                    style={{ color: "white" }}
                    to="configuration/message_templates"
                  >
                    {"Cancel"}
                  </Link>
                </Button>
              </GridItem>
            </GridContainer>
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
