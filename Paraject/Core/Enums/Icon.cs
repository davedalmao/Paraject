using System.ComponentModel;

namespace Paraject.Core.Enums
{
    /// <summary>
    /// Stores the filepath for custom MessageBox Icons
    /// </summary>
    public enum Icon
    {
        //User
        [Description("/UiDesign/Icons/MessageBox/User.svg")]
        User,
        [Description("/UiDesign/Icons/MessageBox/ValidUser.svg")]
        ValidUser,
        [Description("/UiDesign/Icons/MessageBox/InvalidUser.svg")]
        InvalidUser,

        //ProjectIdea
        [Description("/UiDesign/Icons/MessageBox/ProjectIdea.svg")]
        ProjectIdea,
        [Description("/UiDesign/Icons/MessageBox/ValidProjectIdea.svg")]
        ValidProjectIdea,
        [Description("/UiDesign/Icons/MessageBox/InvalidProjectIdea.svg")]
        InvalidProjectIdea,

        //Project
        [Description("/UiDesign/Icons/MessageBox/Project.svg")]
        Project,
        [Description("/UiDesign/Icons/MessageBox/ValidProject.svg")]
        ValidProject,
        [Description("/UiDesign/Icons/MessageBox/InvalidProject.svg")]
        InvalidProject,

        //Note
        [Description("/UiDesign/Icons/MessageBox/Note.svg")]
        Note,
        [Description("/UiDesign/Icons/MessageBox/ValidNote.svg")]
        ValidNote,
        [Description("/UiDesign/Icons/MessageBox/InvalidNote.svg")]
        InvalidNote,

        //Task
        [Description("/UiDesign/Icons/MessageBox/Task.svg")]
        Task,
        [Description("/UiDesign/Icons/MessageBox/ValidTask.svg")]
        ValidTask,
        [Description("/UiDesign/Icons/MessageBox/InvalidTask.svg")]
        InvalidTask,

        //Subtask
        [Description("/UiDesign/Icons/MessageBox/Subtask.svg")]
        Subtask,
        [Description("/UiDesign/Icons/MessageBox/ValidSubtask.svg")]
        ValidSubtask,
        [Description("/UiDesign/Icons/MessageBox/InvalidSubtask.svg")]
        InvalidSubtask
    }
}
