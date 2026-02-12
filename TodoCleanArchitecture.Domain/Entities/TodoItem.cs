using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Domain.Entities
{
    public class TodoItem
    {
        public int Id { get; private set; }

        public string Title { get; private set; }

        public bool IsCompleted { get; private set; }

        public DateTime CreatedAt { get; private set; }

        // 기본 생성자 (EF Core 같은 도구를 위해 필요)
        protected TodoItem() { }

        // Todo를 새로 만들 때 사용하는 생성자
        public TodoItem(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            Title = title;
            IsCompleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        // Todo 완료 처리
        public void Complete()
        {
            IsCompleted = true;
        }
    }
}
